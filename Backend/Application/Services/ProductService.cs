using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;
using Application.Interfaces;
using FluentValidation;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<Product> _validator;
    public ProductService(IProductRepository productRepository, IValidator<Product> validator){
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetProductsByTenantAsync(Guid tenantId)
    {
        // Se buscan todos los productos del tenant
        var products = await _productRepository.GetAllAsync(p => p.TenantId == tenantId && p.IsActive);
        
        //se mapea los productos al DTO
        return products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Barcode = p.Barcode,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            MinimumStock = p.MinimumStock,
            IsActive = p.IsActive
        });
    }

    public async Task<ProductResponseDto?> GetProductByBarcodeAsync(string barcode, Guid tenantId){
        var p = await _productRepository.GetByBarcodeAsync(barcode);
        if (p == null) return null;

        return new ProductResponseDto
        {
            Id = p.Id,
            Barcode = p.Barcode,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            MinimumStock = p.MinimumStock,
            IsActive = p.IsActive
        };
    }

    public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto){

        var newProduct = new Product
        {
            Barcode = dto.Barcode,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            MinimumStock = dto.MinimumStock,
        };
        var validationResult = await _validator.ValidateAsync(newProduct);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        await _productRepository.AddAsync(newProduct);
        await _productRepository.SaveChangesAsync();

        return new ProductResponseDto
        {
            Id = newProduct.Id,
            Barcode = newProduct.Barcode,
            Name = newProduct.Name,
            Description = newProduct.Description,
            Price = newProduct.Price,
            Stock = newProduct.Stock,
            MinimumStock = newProduct.MinimumStock,
            IsActive = newProduct.IsActive
        };
    }

    public async Task<bool> DeleteProductAsync(Guid id){
        // El repositorio lo busca (EF Core lo filtra por Tenant automáticamente)
         var product = await _productRepository.GetByIdAsync(id); 
    
        if (product == null) return false;

        // Baja lógica: no se borra, se desactiva
        product.IsActive = false;

        await _productRepository.SaveChangesAsync();
        return true;
    }
    public async Task UpdateProductAsync(Guid productId, UpdateProductDto dto){
        
        //se busca el producto y se modifica 
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new KeyNotFoundException("El producto especificado no existe o no tenés permisos para verlo.");

        product.UpdateDetails(dto.Name, dto.Barcode, dto.Price, dto.StockMinimum, dto.Description);

        await _productRepository.SaveChangesAsync();
    }
}