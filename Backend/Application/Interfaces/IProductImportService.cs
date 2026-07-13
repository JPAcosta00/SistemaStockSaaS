using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IProductImportService
{
    Task<ProductImportResultDto> ImportFromExcelAsync(IFormFile file);
}