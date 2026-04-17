using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaSystem.Infrastructure.Repositories.Ef;

public class EfUserRepository(AppDbContext db) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync() =>
        await db.Usuarios.Include(u => u.Role).ThenInclude(r => r!.Permissions).ToListAsync();

    public async Task<User?> GetByIdAsync(int id) =>
        await db.Usuarios.Include(u => u.Role).ThenInclude(r => r!.Permissions).FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await db.Usuarios.Include(u => u.Role).ThenInclude(r => r!.Permissions).FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User> AddAsync(User user)
    {
        db.Usuarios.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        db.Usuarios.Update(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await db.Usuarios.FindAsync(id);
        if (user != null)
        {
            user.IsActive = false; // Soft delete
            await db.SaveChangesAsync();
        }
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string passwordHash)
    {
        return await db.Usuarios.AnyAsync(u => u.Username == username && u.PasswordHash == passwordHash && u.IsActive);
    }
}

public class EfRoleRepository(AppDbContext db) : IRoleRepository
{
    public async Task<IEnumerable<Role>> GetAllAsync() =>
        await db.Roles.Include(r => r.Permissions).ToListAsync();

    public async Task<Role?> GetByIdAsync(int id) =>
        await db.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Id == id);
}
