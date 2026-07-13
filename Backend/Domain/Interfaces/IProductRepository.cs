using Domain.Entities;

namespace Domain.Interfaces;

// Hereda de IGenericRepository, por lo que ya tiene incorporado Add, Update, Delete, etc.
public interface IProductRepository : IGenericRepository<Product>
{
    // Método específico: Busca por código de barras dentro del supermercado (Tenant) actual
    Task<Product?> GetByBarcodeAsync(string barcode);

    //crea el mismo getById con otra implementacion
    new Task<Product?> GetByIdAsync(Guid id);
}