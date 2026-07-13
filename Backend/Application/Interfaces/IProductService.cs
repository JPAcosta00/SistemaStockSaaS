using Application.DTOs;

namespace Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetProductsByTenantAsync(Guid tenantId);
    Task<ProductResponseDto?> GetProductByBarcodeAsync(string barcode, Guid tenantId);
    Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto);
    Task<bool> DeleteProductAsync(Guid id);
    Task UpdateProductAsync(Guid productId, UpdateProductDto dto);
}

//contratos para obtener productos y crear