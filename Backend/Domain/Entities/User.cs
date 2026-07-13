using Domain.Interfaces;

namespace Domain.Entities;

public class User : IMustHaveTenant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    //Resolver la parte de los roles por defecto
    public string Role { get; set; } = " - "; 
    public bool IsActive { get; set; } = true;

    // FK al Tenant: Cada usuario pertenece a UN SOLO supermercado
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
}