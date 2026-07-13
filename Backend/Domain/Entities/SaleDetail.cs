
using Domain.Interfaces;
namespace Domain.Entities;

public class SaleDetail
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal => Quantity * UnitPrice; // Campo calculado automático

    // Propiedades de navegación para Entity Framework
    public Sale Sale { get; set; } = null!;
    public Product Product { get; set; } = null!;
}