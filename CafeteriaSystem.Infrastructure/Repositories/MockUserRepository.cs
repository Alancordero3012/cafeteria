using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.MockData;

namespace CafeteriaSystem.Infrastructure.Repositories;

public class MockUserRepository : IUserRepository
{
    private readonly List<User> _users = DataSeeder.Users();

    public Task<IEnumerable<User>> GetAllAsync() => Task.FromResult<IEnumerable<User>>(_users);
    public Task<User?> GetByIdAsync(int id) => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    public Task<User?> GetByUsernameAsync(string username) => Task.FromResult(_users.FirstOrDefault(u => u.Username == username && u.IsActive));

    public Task<bool> ValidateCredentialsAsync(string username, string hash) =>
        Task.FromResult(_users.Any(u => u.Username == username && u.PasswordHash == hash && u.IsActive));

    public Task<User> AddAsync(User user)
    {
        user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User> UpdateAsync(User user)
    {
        var idx = _users.FindIndex(u => u.Id == user.Id);
        if (idx >= 0) _users[idx] = user;
        return Task.FromResult(user);
    }

    public Task DeleteAsync(int id)
    {
        var u = _users.FirstOrDefault(x => x.Id == id);
        if (u != null) u.IsActive = false;
        return Task.CompletedTask;
    }
}

public class MockRoleRepository : IRoleRepository
{
    private readonly List<Role> _roles = DataSeeder.Roles();
    public Task<IEnumerable<Role>> GetAllAsync() => Task.FromResult<IEnumerable<Role>>(_roles);
    public Task<Role?> GetByIdAsync(int id) => Task.FromResult(_roles.FirstOrDefault(r => r.Id == id));
}

public class MockCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = DataSeeder.Customers();

    public Task<IEnumerable<Customer>> GetAllAsync() => Task.FromResult<IEnumerable<Customer>>(_customers);
    public Task<Customer?> GetByIdAsync(int id) => Task.FromResult(_customers.FirstOrDefault(c => c.Id == id));
    public Task<IEnumerable<Customer>> SearchAsync(string term) =>
        Task.FromResult<IEnumerable<Customer>>(_customers.Where(c => c.IsActive && c.Name.Contains(term, StringComparison.OrdinalIgnoreCase)));

    public Task<Customer> AddAsync(Customer customer)
    {
        customer.Id = _customers.Any() ? _customers.Max(c => c.Id) + 1 : 1;
        _customers.Add(customer);
        return Task.FromResult(customer);
    }

    public Task<Customer> UpdateAsync(Customer customer)
    {
        var idx = _customers.FindIndex(c => c.Id == customer.Id);
        if (idx >= 0) _customers[idx] = customer;
        return Task.FromResult(customer);
    }

    public Task DeleteAsync(int id)
    {
        var c = _customers.FirstOrDefault(x => x.Id == id);
        if (c != null) c.IsActive = false;
        return Task.CompletedTask;
    }
}

public class MockSupplierRepository : ISupplierRepository
{
    private readonly List<Supplier> _suppliers = DataSeeder.Suppliers();
    public Task<IEnumerable<Supplier>> GetAllAsync() => Task.FromResult<IEnumerable<Supplier>>(_suppliers);
    public Task<Supplier?> GetByIdAsync(int id) => Task.FromResult(_suppliers.FirstOrDefault(s => s.Id == id));
    public Task<Supplier> AddAsync(Supplier s) { s.Id = _suppliers.Any() ? _suppliers.Max(x => x.Id) + 1 : 1; _suppliers.Add(s); return Task.FromResult(s); }
    public Task<Supplier> UpdateAsync(Supplier s) { var i = _suppliers.FindIndex(x => x.Id == s.Id); if (i >= 0) _suppliers[i] = s; return Task.FromResult(s); }
    public Task DeleteAsync(int id) { var s = _suppliers.FirstOrDefault(x => x.Id == id); if (s != null) s.IsActive = false; return Task.CompletedTask; }
}
