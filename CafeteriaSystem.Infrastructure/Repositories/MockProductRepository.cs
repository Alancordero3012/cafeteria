using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.MockData;

namespace CafeteriaSystem.Infrastructure.Repositories;

public class MockProductRepository : IProductRepository
{
    private readonly List<Product> _products = DataSeeder.Products();

    public Task<IEnumerable<Product>> GetAllAsync() => Task.FromResult<IEnumerable<Product>>(_products);
    public Task<IEnumerable<Product>> GetActivesAsync() => Task.FromResult<IEnumerable<Product>>(_products.Where(p => p.IsActive));
    public Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId) => Task.FromResult<IEnumerable<Product>>(_products.Where(p => p.CategoryId == categoryId && p.IsActive));
    public Task<IEnumerable<Product>> SearchAsync(string term) => Task.FromResult<IEnumerable<Product>>(_products.Where(p => p.IsActive && (p.Name.Contains(term, StringComparison.OrdinalIgnoreCase) || p.Barcode.Contains(term))));
    public Task<Product?> GetByIdAsync(int id) => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
    public Task<Product?> GetByBarcodeAsync(string barcode) => Task.FromResult(_products.FirstOrDefault(p => p.Barcode == barcode));
    public Task<IEnumerable<Product>> GetLowStockAsync() => Task.FromResult<IEnumerable<Product>>(_products.Where(p => p.HasLowStock && p.IsActive));

    public Task<Product> AddAsync(Product product)
    {
        product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
        product.CreatedAt = DateTime.Now;
        _products.Add(product);
        return Task.FromResult(product);
    }

    public Task<Product> UpdateAsync(Product product)
    {
        var idx = _products.FindIndex(p => p.Id == product.Id);
        if (idx >= 0) { product.UpdatedAt = DateTime.Now; _products[idx] = product; }
        return Task.FromResult(product);
    }

    public Task DeleteAsync(int id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        if (p != null) p.IsActive = false;
        return Task.CompletedTask;
    }

    public Task UpdateStockAsync(int productId, int quantity)
    {
        var p = _products.FirstOrDefault(x => x.Id == productId);
        if (p != null) { p.CurrentStock += quantity; p.UpdatedAt = DateTime.Now; }
        return Task.CompletedTask;
    }
}
