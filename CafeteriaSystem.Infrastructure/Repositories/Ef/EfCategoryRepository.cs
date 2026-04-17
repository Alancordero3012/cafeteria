using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaSystem.Infrastructure.Repositories.Ef;

public class EfCategoryRepository(AppDbContext db) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllAsync() =>
        await db.Categorias.Where(c => c.IsActive).ToListAsync();

    public async Task<Category?> GetByIdAsync(int id) =>
        await db.Categorias.FindAsync(id);

    public async Task<Category> AddAsync(Category category)
    {
        db.Categorias.Add(category);
        await db.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        db.Categorias.Update(category);
        await db.SaveChangesAsync();
        return category;
    }

    public async Task DeleteAsync(int id)
    {
        var cat = await db.Categorias.FindAsync(id);
        if (cat != null)
        {
            cat.IsActive = false;
            await db.SaveChangesAsync();
        }
    }
}
