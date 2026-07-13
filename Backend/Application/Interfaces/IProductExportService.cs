using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductExportService
    {
        //arma los archivos de cada tipo
        Task<byte[]> GenerateExcelAsync(List<Product> productIds);
        Task<byte[]> GeneratePdfAsync(List<Product> productIds);

        //realiza todos los filtros 
        Task<byte[]> ExportProductsToPdfAsync(ProductReportFilterDto filters, Guid tenantId);
        Task<byte[]> ExportProductsToExcelAsync(ProductReportFilterDto filters, Guid tenantId);
    }
}