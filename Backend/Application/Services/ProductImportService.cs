using Application.DTOs;
using Application.Interfaces;
using ClosedXML.Excel;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class ProductImportService : IProductImportService
{
    private readonly IProductRepository _productRepository; 
    private readonly IValidator<Product> _productValidator;

    public ProductImportService(IProductRepository productRepository, IValidator<Product> productValidator)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
    }

    public async Task<ProductImportResultDto> ImportFromExcelAsync(IFormFile file)
    {
        var result = new ProductImportResultDto();
        var productsToInsert = new List<Product>();

        if (file == null || file.Length == 0)
            throw new ArgumentException("El archivo de Excel está vacío o es inválido.");

        // Se lee el archivo directamente desde el Stream de memoria
        using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.First();

        // Valido si la hoja tiene celdas usadas
        var usedRange = worksheet.RangeUsed();
        if (usedRange == null){
            throw new ArgumentException("La planilla de Excel no contiene datos.");
        }

        var rows = usedRange.RowsUsed().Skip(1); 

        int currentRowIndex = 1;

        //recorre el el archivo fila por fila, arma los productos temporales y los carga en una lista 
        foreach (var row in rows){
            currentRowIndex++;
            result.TotalRows++;

            // extrae las celdas
            string barcode = row.Cell(1).GetValue<string>().Trim();
            string name = row.Cell(2).GetValue<string>().Trim();
            string description = row.Cell(3).GetValue<string>().Trim();
            decimal price = row.Cell(4).GetValue<decimal>();
            int stock = row.Cell(5).GetValue<int>();

            // Se arma el producto temporal
            var product = new Product
            {
                Barcode = barcode,
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                IsActive = true
                // El TenantId se va a inyectar solo en el DbContext 
            };

            var validationResult = await _productValidator.ValidateAsync(product);

            if (!validationResult.IsValid)
            {
                result.FailedRows++;
                result.Errors.Add(new RowErrorDto
                {
                    RowNumber = currentRowIndex,
                    Barcode = barcode,
                    ErrorMessage = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage))
                });
                continue; 
            }

            productsToInsert.Add(product);
            result.SuccessfulRows++;
        }

        // recorre la lista de productos para cargar en el inventario
        if (productsToInsert.Any()){
             foreach (var product in productsToInsert) {
                await _productRepository.AddAsync(product); 
            }
    
            //se hace una sola vez, xq EF junta toda la informacion y la guarda de una 
            await _productRepository.SaveChangesAsync();
        }

        return result;
    }
}