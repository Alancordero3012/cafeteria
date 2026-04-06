using CafeteriaSystem.Domain.Entities;

namespace CafeteriaSystem.Domain.Interfaces;

public interface IInventoryRepository
{
    Task<IEnumerable<Inventory>> GetAllAsync();
    Task<Inventory?> GetByProductIdAsync(int productId);
    Task<Inventory> AddAsync(Inventory inventory);
    Task<Inventory> UpdateAsync(Inventory inventory);
    Task AdjustStockAsync(int productId, int quantity, string reason);
}

public interface IKardexRepository
{
    Task<IEnumerable<KardexEntry>> GetByProductAsync(int productId);
    Task<IEnumerable<KardexEntry>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<KardexEntry> AddAsync(KardexEntry entry);
}
