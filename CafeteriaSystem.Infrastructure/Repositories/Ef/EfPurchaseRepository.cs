using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaSystem.Infrastructure.Repositories.Ef;

public class EfPurchaseRepository(AppDbContext db) : IPurchaseRepository
{
    public async Task<IEnumerable<Purchase>> GetAllAsync() =>
        await db.Compras
            .Include(p => p.Supplier)
            .Include(p => p.User)
            .Include(p => p.Details).ThenInclude(d => d.Product)
            .OrderByDescending(p => p.Date)
            .ToListAsync();

    public async Task<Purchase?> GetByIdAsync(int id) =>
        await db.Compras
            .Include(p => p.Supplier)
            .Include(p => p.User)
            .Include(p => p.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Purchase> AddAsync(Purchase purchase)
    {
        db.Compras.Add(purchase);
        await db.SaveChangesAsync();
        return purchase;
    }

    public async Task<Purchase> UpdateAsync(Purchase purchase)
    {
        db.Compras.Update(purchase);
        await db.SaveChangesAsync();
        return purchase;
    }

    public async Task<int> GetNextPurchaseNumberAsync()
    {
        var last = await db.Compras.MaxAsync(p => (int?)p.Id) ?? 0;
        return last + 1;
    }
}

public class EfCashRegisterRepository(AppDbContext db) : ICashRegisterRepository
{
    public async Task<CashRegister?> GetOpenRegisterAsync() =>
        await db.ArqueosCaja
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Status == CashRegisterStatus.Abierta);

    public async Task<IEnumerable<CashRegister>> GetAllAsync() =>
        await db.ArqueosCaja.Include(a => a.User)
            .OrderByDescending(a => a.OpenDate).ToListAsync();

    public async Task<CashRegister?> GetByIdAsync(int id) =>
        await db.ArqueosCaja.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);

    public async Task<CashRegister> AddAsync(CashRegister cashRegister)
    {
        db.ArqueosCaja.Add(cashRegister);
        await db.SaveChangesAsync();
        return cashRegister;
    }

    public async Task<CashRegister> UpdateAsync(CashRegister cashRegister)
    {
        db.ArqueosCaja.Update(cashRegister);
        await db.SaveChangesAsync();
        return cashRegister;
    }
}

public class EfDiscountRepository : IDiscountRepository
{
    // Descuentos no tienen tabla en el esquema SQL actual.
    // Se retorna vacío hasta que se agregue la tabla.
    public Task<IEnumerable<Discount>> GetAllActiveAsync() =>
        Task.FromResult<IEnumerable<Discount>>(Array.Empty<Discount>());

    public Task<Discount?> GetByCodeAsync(string code) =>
        Task.FromResult<Discount?>(null);

    public Task<Discount?> GetByIdAsync(int id) =>
        Task.FromResult<Discount?>(null);

    public Task<Discount> AddAsync(Discount discount) =>
        Task.FromResult(discount);

    public Task<Discount> UpdateAsync(Discount discount) =>
        Task.FromResult(discount);
}

public class EfTaxRateRepository : ITaxRateRepository
{
    // TaxRate no tiene tabla en el esquema SQL (se usa aplica_itbis en Productos).
    // Se retorna un registro por defecto (ITBIS 18%).
    private static readonly TaxRate _defaultItbis = new() { Id = 1, Name = "ITBIS", Rate = 0.18m, IsDefault = true, IsActive = true };

    public Task<IEnumerable<TaxRate>> GetAllAsync() =>
        Task.FromResult<IEnumerable<TaxRate>>(new[] { _defaultItbis });

    public Task<TaxRate?> GetDefaultAsync() =>
        Task.FromResult<TaxRate?>(_defaultItbis);

    public Task<TaxRate> UpdateAsync(TaxRate taxRate) =>
        Task.FromResult(taxRate);
}
