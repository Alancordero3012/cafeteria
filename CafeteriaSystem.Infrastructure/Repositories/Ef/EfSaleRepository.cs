using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaSystem.Infrastructure.Repositories.Ef;

public class EfSaleRepository(AppDbContext db) : ISaleRepository
{
    public async Task<IEnumerable<Sale>> GetAllAsync() =>
        await db.Ventas
            .Include(s => s.Customer)
            .Include(s => s.User)
            .Include(s => s.PaymentMethodEntity)
            .Include(s => s.Details).ThenInclude(d => d.Product)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

    public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime from, DateTime to) =>
        await db.Ventas
            .Include(s => s.Customer)
            .Include(s => s.PaymentMethodEntity)
            .Include(s => s.Details).ThenInclude(d => d.Product)
            .Where(s => s.SaleDate >= from && s.SaleDate <= to)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

    public async Task<IEnumerable<Sale>> GetTodayAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        return await GetByDateRangeAsync(today, tomorrow);
    }

    public async Task<Sale?> GetByIdAsync(int id) =>
        await db.Ventas
            .Include(s => s.Customer)
            .Include(s => s.User)
            .Include(s => s.PaymentMethodEntity)
            .Include(s => s.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Sale?> GetByNumberAsync(string saleNumber) =>
        await db.Ventas
            .Include(s => s.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);

    public async Task<Sale> AddAsync(Sale sale)
    {
        db.Ventas.Add(sale);
        await db.SaveChangesAsync();
        return sale;
    }

    public async Task<Sale> UpdateAsync(Sale sale)
    {
        db.Ventas.Update(sale);
        await db.SaveChangesAsync();
        return sale;
    }

    public async Task<int> GetNextSaleNumberAsync()
    {
        var last = await db.Ventas.MaxAsync(s => (int?)s.Id) ?? 0;
        return last + 1;
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime from, DateTime to) =>
        await db.Ventas
            .Where(s => s.SaleDate >= from && s.SaleDate <= to && s.Status == SaleStatus.Completada)
            .SumAsync(s => s.Total);

    public async Task<IEnumerable<Sale>> GetByCustomerAsync(int customerId) =>
        await db.Ventas
            .Include(s => s.Details).ThenInclude(d => d.Product)
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
}
