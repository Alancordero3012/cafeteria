using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.MockData;

namespace CafeteriaSystem.Infrastructure.Repositories;

public class MockInventoryRepository : IInventoryRepository
{
    private readonly List<Inventory> _inventory;

    public MockInventoryRepository()
    {
        _inventory = DataSeeder.Products().Select(p => new Inventory
        {
            Id = p.Id,
            ProductId = p.Id,
            Product = p,
            Quantity = p.CurrentStock,
            Location = "Almacén Principal",
            LastUpdated = DateTime.Now
        }).ToList();
    }

    public Task<IEnumerable<Inventory>> GetAllAsync() => Task.FromResult<IEnumerable<Inventory>>(_inventory);
    public Task<Inventory?> GetByProductIdAsync(int productId) => Task.FromResult(_inventory.FirstOrDefault(i => i.ProductId == productId));
    public Task<Inventory> AddAsync(Inventory inv) { _inventory.Add(inv); return Task.FromResult(inv); }

    public Task<Inventory> UpdateAsync(Inventory inv)
    {
        var idx = _inventory.FindIndex(i => i.Id == inv.Id);
        if (idx >= 0) _inventory[idx] = inv;
        return Task.FromResult(inv);
    }

    public Task AdjustStockAsync(int productId, int quantity, string reason)
    {
        var inv = _inventory.FirstOrDefault(i => i.ProductId == productId);
        if (inv != null) { inv.Quantity += quantity; inv.LastUpdated = DateTime.Now; }
        return Task.CompletedTask;
    }
}

public class MockKardexRepository : IKardexRepository
{
    private readonly List<KardexEntry> _entries = [];
    private int _nextId = 1;

    public Task<IEnumerable<KardexEntry>> GetByProductAsync(int productId) =>
        Task.FromResult<IEnumerable<KardexEntry>>(_entries.Where(e => e.ProductId == productId).OrderByDescending(e => e.Date));
    public Task<IEnumerable<KardexEntry>> GetByDateRangeAsync(DateTime from, DateTime to) =>
        Task.FromResult<IEnumerable<KardexEntry>>(_entries.Where(e => e.Date >= from && e.Date < to));
    public Task<KardexEntry> AddAsync(KardexEntry entry) { entry.Id = _nextId++; _entries.Add(entry); return Task.FromResult(entry); }
}

public class MockDiscountRepository : IDiscountRepository
{
    private readonly List<Discount> _discounts = DataSeeder.Discounts();
    public Task<IEnumerable<Discount>> GetAllActiveAsync() => Task.FromResult<IEnumerable<Discount>>(_discounts.Where(d => d.IsValid));
    public Task<Discount?> GetByCodeAsync(string code) => Task.FromResult(_discounts.FirstOrDefault(d => d.Code == code && d.IsValid));
    public Task<Discount?> GetByIdAsync(int id) => Task.FromResult(_discounts.FirstOrDefault(d => d.Id == id));
    public Task<Discount> AddAsync(Discount d) { d.Id = _discounts.Any() ? _discounts.Max(x => x.Id) + 1 : 1; _discounts.Add(d); return Task.FromResult(d); }
    public Task<Discount> UpdateAsync(Discount d) { var i = _discounts.FindIndex(x => x.Id == d.Id); if (i >= 0) _discounts[i] = d; return Task.FromResult(d); }
}

public class MockCashRegisterRepository : ICashRegisterRepository
{
    private readonly List<CashRegister> _registers =
    [
        new(){Id=1, UserId=1, OpenDate=DateTime.Today.AddHours(7), InitialAmount=5000, ExpectedAmount=5000, Status=CashRegisterStatus.Abierta}
    ];

    public Task<CashRegister?> GetOpenRegisterAsync() => Task.FromResult(_registers.FirstOrDefault(r => r.Status == CashRegisterStatus.Abierta));
    public Task<IEnumerable<CashRegister>> GetAllAsync() => Task.FromResult<IEnumerable<CashRegister>>(_registers.OrderByDescending(r => r.OpenDate));
    public Task<CashRegister?> GetByIdAsync(int id) => Task.FromResult(_registers.FirstOrDefault(r => r.Id == id));
    public Task<CashRegister> AddAsync(CashRegister r) { r.Id = _registers.Any() ? _registers.Max(x => x.Id) + 1 : 1; _registers.Add(r); return Task.FromResult(r); }
    public Task<CashRegister> UpdateAsync(CashRegister r) { var i = _registers.FindIndex(x => x.Id == r.Id); if (i >= 0) _registers[i] = r; return Task.FromResult(r); }
}

public class MockPurchaseRepository : IPurchaseRepository
{
    private readonly List<Purchase> _purchases = [];
    private int _nextId = 1;
    public Task<IEnumerable<Purchase>> GetAllAsync() => Task.FromResult<IEnumerable<Purchase>>(_purchases.OrderByDescending(p => p.Date));
    public Task<Purchase?> GetByIdAsync(int id) => Task.FromResult(_purchases.FirstOrDefault(p => p.Id == id));
    public Task<Purchase> AddAsync(Purchase p) { p.Id = _nextId++; _purchases.Add(p); return Task.FromResult(p); }
    public Task<Purchase> UpdateAsync(Purchase p) { var i = _purchases.FindIndex(x => x.Id == p.Id); if (i >= 0) _purchases[i] = p; return Task.FromResult(p); }
    public Task<int> GetNextPurchaseNumberAsync() => Task.FromResult(_nextId);
}
