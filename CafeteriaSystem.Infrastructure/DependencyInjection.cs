using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Data;
using CafeteriaSystem.Infrastructure.Repositories.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CafeteriaSystem.Application;

namespace CafeteriaSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── Base de Datos SQL Server ─────────────────────────────────────────
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null)));

        // ── Repositorios EF Core ─────────────────────────────────────────────
        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<ICategoryRepository, EfCategoryRepository>();
        services.AddScoped<ISaleRepository, EfSaleRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IRoleRepository, EfRoleRepository>();
        services.AddScoped<ICustomerRepository, EfCustomerRepository>();
        services.AddScoped<ISupplierRepository, EfSupplierRepository>();
        services.AddScoped<IInventoryRepository, EfInventoryRepository>();
        services.AddScoped<IKardexRepository, EfKardexRepository>();
        services.AddScoped<IDiscountRepository, EfDiscountRepository>();
        services.AddScoped<ICashRegisterRepository, EfCashRegisterRepository>();
        services.AddScoped<IPurchaseRepository, EfPurchaseRepository>();
        services.AddScoped<ITaxRateRepository, EfTaxRateRepository>();
        services.AddScoped<IActivityLogRepository, EfActivityLogRepository>();

        // ── Capa de Aplicación ───────────────────────────────────────────────
        services.AddApplication();

        return services;
    }
}
