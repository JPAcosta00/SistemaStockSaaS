using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly IInventoryStatsService _statsService;

    public StatsController(IInventoryStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet ("dashboard")]
    public async Task<ActionResult<DashboardDataDto>> GetDashboardStats([FromQuery] string? name,[FromQuery] string? period)
    {
        var tenantClaim = User.FindFirst("TenantId")?.Value;

        if (string.IsNullOrEmpty(tenantClaim))
        {
            return Unauthorized("No se pudo determinar el Tenant del usuario actual.");
        }

        // Convierto el string a guid
        if (!Guid.TryParse(tenantClaim, out Guid tenantId))
        {
            return BadRequest("El identificador del Tenant no es válido.");
        }


        // Mapeao el DTO del producto filtrado
        var filter = new ProductReportFilterDto
        {
            Name = name,
            Period = period
        };

        var stats = await _statsService.GetStatsByInventoryFiltersAsync(tenantId, filter);
        return Ok(stats);
    }
}