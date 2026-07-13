using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

// Hereda la lógica de GenericRepository e implementa el contrato específico IProductRepository
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<Product?> GetByBarcodeAsync(string barcode){     
         return await _context.Products.FirstOrDefaultAsync(p => p.Barcode == barcode && p.IsActive);
    }

    public async new Task<Product?> GetByIdAsync(Guid id){
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }
}