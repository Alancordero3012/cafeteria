using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.MockData;

namespace CafeteriaSystem.Infrastructure.Repositories;

public class MockSaleRepository : ISaleRepository
{
    private readonly List<Sale> _sales = DataSeeder.HistoricalSales();

    public Task<IEnumerable<Sale>> GetAllAsync() => Task.FromResult<IEnumerable<Sale>>(_sales.OrderByDescending(s => s.SaleDate));
    public Task<Sale?> GetByIdAsync(int id) => Task.FromResult(_sales.FirstOrDefault(s => s.Id == id));
    public Task<Sale?> GetByNumberAsync(string num) => Task.FromResult(_sales.FirstOrDefault(s => s.SaleNumber == num));

    public Task<IEnumerable<Sale>> GetTodayAsync()
    {
        var today = DateTime.Today;
        return Task.FromResult<IEnumerable<Sale>>(_sales.Where(s => s.SaleDate.Date == today));
    }

    public Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime from, DateTime to) =>
        Task.FromResult<IEnumerable<Sale>>(_sales.Where(s => s.SaleDate >= from && s.SaleDate < to));

    public Task<IEnumerable<Sale>> GetByCustomerAsync(int customerId) =>
        Task.FromResult<IEnumerable<Sale>>(_sales.Where(s => s.CustomerId == customerId));

    public Task<Sale> AddAsync(Sale sale)
    {
        sale.Id = _sales.Any() ? _sales.Max(s => s.Id) + 1 : 1;
        _sales.Add(sale);
        return Task.FromResult(sale);
    }

    public Task<Sale> UpdateAsync(Sale sale)
    {
        var idx = _sales.FindIndex(s => s.Id == sale.Id);
        if (idx >= 0) _sales[idx] = sale;
        return Task.FromResult(sale);
    }

    public Task<int> GetNextSaleNumberAsync() => Task.FromResult(_sales.Any() ? _sales.Max(s => s.Id) + 1 : 1);
    public Task<decimal> GetTotalRevenueAsync(DateTime from, DateTime to) =>
        Task.FromResult(_sales.Where(s => s.SaleDate >= from && s.SaleDate < to && s.Status == SaleStatus.Completada).Sum(s => s.Total));
}
