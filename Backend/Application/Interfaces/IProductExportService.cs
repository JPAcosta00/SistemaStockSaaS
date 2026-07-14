using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductExportService
    {
        //arma los archivos de cada tipo
        Task<byte[]> GenerateExcelAsync(IEnumerable<Product> products);
        Task<byte[]> GeneratePdfAsync(IEnumerable<Product> products);

    }
}