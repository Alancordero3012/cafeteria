using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaSystem.Infrastructure.Repositories.Ef;

public class EfCustomerRepository(AppDbContext db) : ICustomerRepository
{
    public async Task<IEnumerable<Customer>> GetAllAsync() =>
        await db.Clientes.Where(c => c.IsActive).ToListAsync();

    public async Task<Customer?> GetByIdAsync(int id) =>
        await db.Clientes.FindAsync(id);

    public async Task<IEnumerable<Customer>> SearchAsync(string term) =>
        await db.Clientes.Where(c => c.IsActive &&
            (c.Name.Contains(term) || c.Cedula.Contains(term) || c.Phone.Contains(term)))
            .ToListAsync();

    public async Task<Customer> AddAsync(Customer customer)
    {
        db.Clientes.Add(customer);
        await db.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        db.Clientes.Update(customer);
        await db.SaveChangesAsync();
        return customer;
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await db.Clientes.FindAsync(id);
        if (customer != null)
        {
            customer.IsActive = false;
            await db.SaveChangesAsync();
        }
    }
}

public class EfSupplierRepository(AppDbContext db) : ISupplierRepository
{
    public async Task<IEnumerable<Supplier>> GetAllAsync() =>
        await db.Proveedores.Where(s => s.IsActive).ToListAsync();

    public async Task<Supplier?> GetByIdAsync(int id) =>
        await db.Proveedores.FindAsync(id);

    public async Task<Supplier> AddAsync(Supplier supplier)
    {
        db.Proveedores.Add(supplier);
        await db.SaveChangesAsync();
        return supplier;
    }

    public async Task<Supplier> UpdateAsync(Supplier supplier)
    {
        db.Proveedores.Update(supplier);
        await db.SaveChangesAsync();
        return supplier;
    }

    public async Task DeleteAsync(int id)
    {
        var supplier = await db.Proveedores.FindAsync(id);
        if (supplier != null)
        {
            supplier.IsActive = false;
            await db.SaveChangesAsync();
        }
    }
}
