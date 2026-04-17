using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaSystem.Infrastructure.Repositories.Ef;

public class EfActivityLogRepository(AppDbContext db) : IActivityLogRepository
{
    public async Task AddAsync(ActivityLog log)
    {
        db.Logs.Add(log);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ActivityLog>> GetRecentAsync(int count = 50) =>
        await db.Logs
            .OrderByDescending(l => l.Date)
            .Take(count)
            .ToListAsync();
}
