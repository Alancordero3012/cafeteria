using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaSystem.Infrastructure.Repositories.Ef;

public class EfProductRepository(AppDbContext db) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await db.Productos.Include(p => p.Category).Include(p => p.Supplier).ToListAsync();

    public async Task<IEnumerable<Product>> GetActivesAsync() =>
        await db.Productos.Include(p => p.Category).Include(p => p.Supplier)
            .Where(p => p.IsActive).ToListAsync();

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId) =>
        await db.Productos.Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsActive).ToListAsync();

    public async Task<IEnumerable<Product>> SearchAsync(string term) =>
        await db.Productos.Include(p => p.Category)
            .Where(p => p.IsActive && (p.Name.Contains(term) || p.Barcode.Contains(term) || p.Description.Contains(term)))
            .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await db.Productos.Include(p => p.Category).Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product?> GetByBarcodeAsync(string barcode) =>
        await db.Productos.Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Barcode == barcode && p.IsActive);

    public async Task<Product> AddAsync(Product product)
    {
        product.UpdatedAt = DateTime.Now;
        db.Productos.Add(product);
        await db.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        product.UpdatedAt = DateTime.Now;
        db.Productos.Update(product);
        await db.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(int id)
    {
        var product = await db.Productos.FindAsync(id);
        if (product != null)
        {
            product.IsActive = false;
            product.UpdatedAt = DateTime.Now;
            await db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetLowStockAsync() =>
        await db.Productos.Include(p => p.Category)
            .Where(p => p.IsActive && p.CurrentStock <= p.MinimumStock).ToListAsync();

    public async Task UpdateStockAsync(int productId, int quantity)
    {
        var product = await db.Productos.FindAsync(productId);
        if (product != null)
        {
            product.CurrentStock += quantity;
            product.UpdatedAt = DateTime.Now;
            await db.SaveChangesAsync();
        }
    }
}
