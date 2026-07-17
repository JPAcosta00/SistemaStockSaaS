using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize] // Protegido para que solo usuarios autenticados operen
    [ApiController]
    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        // 1. GET: api/sales (Historial)
        [HttpGet]
        public async Task<IActionResult> GetHistory(){
        // NOTA: Acá necesitás obtener el TenantId del usuario logueado.
        // Si usás JWT, lo ideal es sacarlo de los Claims. Si no, temporalmente podés usar uno de prueba.
        var tenantIdClaim = User.FindFirst("TenantId")?.Value; 
        if (string.IsNullOrEmpty(tenantIdClaim) || !Guid.TryParse(tenantIdClaim, out Guid tenantId))
                {
                    return Unauthorized(new { Message = "El identificador de organización (Tenant) no es válido o no está presente." });
                }

        var history = await _saleService.GetSalesHistoryAsync(tenantId);
        return Ok(history);
    }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleDto dto)
        {
            try
            {
                // Extraemos el TenantId de los Claims del usuario autenticado (JWT)
                var tenantIdClaim = User.FindFirst("TenantId")?.Value;
                
                if (string.IsNullOrEmpty(tenantIdClaim) || !Guid.TryParse(tenantIdClaim, out Guid tenantId))
                {
                    return Unauthorized(new { Message = "El identificador de organización (Tenant) no es válido o no está presente." });
                }

                // Delegamos la lógica transaccional al servicio de aplicación
                var saleId = await _saleService.CreateSaleAsync(tenantId, dto);

                return Ok(new { Message = "Venta registrada con éxito e inventario actualizado.", SaleId = saleId });
            }
            catch (ArgumentException ex)
            {
                // Errores de validación de datos de entrada
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Errores de negocio (Ej: Stock insuficiente capturado en el loop del servicio)
                return BadRequest(new { Message = ex.Message });
            }
        }
    
    }
}