using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infraestructure.Security;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetTenantId()
    {
        // busca el tenant id del usuario autenticado por token
        var claimTenantId = _httpContextAccessor.HttpContext?.User?
            .FindFirst("TenantId")?.Value;

        if (Guid.TryParse(claimTenantId, out var tenantGuid))
        {
            return tenantGuid;
        }

        // Si no esta en el token, se usa en el header HTTP
        var headerTenantId = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"].ToString();

        if (Guid.TryParse(headerTenantId, out var headerGuid))
        {
            return headerGuid;
        }

        return null;
    }
}