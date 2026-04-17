using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaSystem.Infrastructure.Repositories.Ef;

/// <summary>
/// Implementa IInventoryRepository sobre la tabla Productos.
/// En el esquema sistema_ventasbd el stock vive en Productos.stock_actual,
/// no hay una tabla Inventario separada.
/// </summary>
public class EfInventoryRepository(AppDbContext db) : IInventoryRepository
{
    public async Task<IEnumerable<Inventory>> GetAllAsync()
    {
        var products = await db.Productos.Include(p => p.Category).ToListAsync();
        return products.Select(p => MapToInventory(p));
    }

    public async Task<Inventory?> GetByProductIdAsync(int productId)
    {
        var product = await db.Productos.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);
        return product == null ? null : MapToInventory(product);
    }

    public async Task<Inventory> AddAsync(Inventory inventory)
    {
        // Actualiza el stock en el producto directamente
        var product = await db.Productos.FindAsync(inventory.ProductId);
        if (product != null)
        {
            product.CurrentStock = inventory.Quantity;
            product.UpdatedAt = DateTime.Now;
            await db.SaveChangesAsync();
        }
        return inventory;
    }

    public async Task<Inventory> UpdateAsync(Inventory inventory)
    {
        var product = await db.Productos.FindAsync(inventory.ProductId);
        if (product != null)
        {
            product.CurrentStock = inventory.Quantity;
            product.UpdatedAt = DateTime.Now;
            await db.SaveChangesAsync();
        }
        return inventory;
    }

    public async Task AdjustStockAsync(int productId, int quantity, string reason)
    {
        var product = await db.Productos.FindAsync(productId);
        if (product == null) return;

        var stockBefore = product.CurrentStock;
        product.CurrentStock += quantity;
        product.UpdatedAt = DateTime.Now;

        // Registra el movimiento en Kardex
        db.Kardex.Add(new KardexEntry
        {
            ProductId = productId,
            MovementType = quantity >= 0 ? "Entrada" : "Salida",
            Quantity = Math.Abs(quantity),
            StockBefore = stockBefore,
            StockAfter = product.CurrentStock,
            Date = DateTime.Now,
            Reason = reason
        });

        await db.SaveChangesAsync();
    }

    private static Inventory MapToInventory(Product p) => new()
    {
        Id = p.Id,
        ProductId = p.Id,
        Product = p,
        Quantity = (int)p.CurrentStock,
        LastUpdated = p.UpdatedAt ?? p.CreatedAt
    };
}

public class EfKardexRepository(AppDbContext db) : IKardexRepository
{
    public async Task<IEnumerable<KardexEntry>> GetByProductAsync(int productId) =>
        await db.Kardex.Include(k => k.Product)
            .Where(k => k.ProductId == productId)
            .OrderByDescending(k => k.Date)
            .ToListAsync();

    public async Task<IEnumerable<KardexEntry>> GetByDateRangeAsync(DateTime from, DateTime to) =>
        await db.Kardex.Include(k => k.Product)
            .Where(k => k.Date >= from && k.Date <= to)
            .OrderByDescending(k => k.Date)
            .ToListAsync();

    public async Task<KardexEntry> AddAsync(KardexEntry entry)
    {
        db.Kardex.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }
}
