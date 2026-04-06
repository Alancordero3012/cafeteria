using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.MockData;

namespace CafeteriaSystem.Infrastructure.Repositories;

public class MockCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories = DataSeeder.Categories();

    public Task<IEnumerable<Category>> GetAllAsync() => Task.FromResult<IEnumerable<Category>>(_categories);
    public Task<Category?> GetByIdAsync(int id) => Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));

    public Task<Category> AddAsync(Category category)
    {
        category.Id = _categories.Any() ? _categories.Max(c => c.Id) + 1 : 1;
        _categories.Add(category);
        return Task.FromResult(category);
    }

    public Task<Category> UpdateAsync(Category category)
    {
        var idx = _categories.FindIndex(c => c.Id == category.Id);
        if (idx >= 0) _categories[idx] = category;
        return Task.FromResult(category);
    }

    public Task DeleteAsync(int id)
    {
        var c = _categories.FirstOrDefault(x => x.Id == id);
        if (c != null) c.IsActive = false;
        return Task.CompletedTask;
    }
}

public class MockTaxRateRepository : ITaxRateRepository
{
    private readonly List<TaxRate> _rates = DataSeeder.TaxRates();

    public Task<IEnumerable<TaxRate>> GetAllAsync() => Task.FromResult<IEnumerable<TaxRate>>(_rates);
    public Task<TaxRate?> GetDefaultAsync() => Task.FromResult(_rates.FirstOrDefault(r => r.IsDefault && r.IsActive));
    public Task<TaxRate> UpdateAsync(TaxRate taxRate)
    {
        var idx = _rates.FindIndex(r => r.Id == taxRate.Id);
        if (idx >= 0) _rates[idx] = taxRate;
        return Task.FromResult(taxRate);
    }
}
