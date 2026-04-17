using CafeteriaSystem.Domain.Entities;

namespace CafeteriaSystem.Domain.Interfaces;

public interface IActivityLogRepository
{
    Task AddAsync(ActivityLog log);
    Task<IEnumerable<ActivityLog>> GetRecentAsync(int count = 50);
}
