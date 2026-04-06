using CafeteriaSystem.Domain.Entities;

namespace CafeteriaSystem.Domain.Interfaces;

public interface ISaleRepository
{
    Task<IEnumerable<Sale>> GetAllAsync();
    Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<IEnumerable<Sale>> GetTodayAsync();
    Task<Sale?> GetByIdAsync(int id);
    Task<Sale?> GetByNumberAsync(string saleNumber);
    Task<Sale> AddAsync(Sale sale);
    Task<Sale> UpdateAsync(Sale sale);
    Task<int> GetNextSaleNumberAsync();
    Task<decimal> GetTotalRevenueAsync(DateTime from, DateTime to);
    Task<IEnumerable<Sale>> GetByCustomerAsync(int customerId);
}
