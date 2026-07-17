using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class InventoryStatsService : IInventoryStatsService
{
    private readonly IProductService _productService;
    private readonly ISaleRepository _saleRepository;

    public InventoryStatsService(IProductService productService, ISaleRepository saleRepository)
    {
        _productService = productService;
        _saleRepository = saleRepository;
    }

    public async Task<DashboardDataDto> GetStatsByInventoryFiltersAsync(Guid tenantId, ProductReportFilterDto filter){
        // reuso el filtro que existe en el servicio de producto
        var filteredProducts = await _productService.GetFilteredProductsAsync(filter, tenantId);
        var filteredProductsList = filteredProducts.ToList();

        // se trae las ventas del ultimo mes
        var startDate = DateTime.UtcNow.AddDays(-30);
        var sales = await _saleRepository.GetSalesWithDetailsAsync(tenantId, startDate);

        // hashSet con los productos filtrados
        var filteredProductIds = filteredProductsList.Select(p => p.Id).ToHashSet();

        // Se filtran los SaleDetail de los productos filtrados antes
        var filteredDetails = sales
            .SelectMany(s => s.Details)
            .Where(d => filteredProductIds.Contains(d.ProductId))
            .ToList();

        
        //calculo de estadisticas (unidades vendidas y demas )
        decimal totalRevenue = filteredDetails.Sum(d => d.Quantity * d.UnitPrice);
        int unitsSold = filteredDetails.Sum(d => d.Quantity);
        
        // Alertas de stock critico 
        int lowStockCount = filteredProductsList.Count(p => p.Stock <= p.MinimumStock);

        // Se agrupan los productos mas vendidos con el filtro que esta activo
        var topProducts = filteredDetails
            .GroupBy(d => d.Product?.Name ?? "Producto Sin Nombre")
            .Select(g => new TopProductDto
            {
                ProductName = g.Key,
                SalesCount = g.Sum(d => d.Quantity),
                TotalAmount = g.Sum(d => d.Quantity * d.UnitPrice)
            })
            .OrderByDescending(p => p.SalesCount)
            .Take(4)
            .ToList();

        return new DashboardDataDto
        {
            Metrics = new DashboardMetricsDto
            {
                TotalRevenue = totalRevenue,
                TotalSalesCount = unitsSold,
                ActiveProductsCount = filteredProductsList.Count, // Cantidad de ítems en la grilla actual
                LowStockAlertsCount = lowStockCount
            },
            TopProducts = topProducts
        };
    }
}