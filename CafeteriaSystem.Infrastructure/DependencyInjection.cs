using CafeteriaSystem.Application;
using CafeteriaSystem.Domain.Interfaces;
using CafeteriaSystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CafeteriaSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Repositories - Singleton so mock data persists across requests
        services.AddSingleton<IProductRepository, MockProductRepository>();
        services.AddSingleton<ICategoryRepository, MockCategoryRepository>();
        services.AddSingleton<ISaleRepository, MockSaleRepository>();
        services.AddSingleton<IUserRepository, MockUserRepository>();
        services.AddSingleton<IRoleRepository, MockRoleRepository>();
        services.AddSingleton<ICustomerRepository, MockCustomerRepository>();
        services.AddSingleton<ISupplierRepository, MockSupplierRepository>();
        services.AddSingleton<IInventoryRepository, MockInventoryRepository>();
        services.AddSingleton<IKardexRepository, MockKardexRepository>();
        services.AddSingleton<IDiscountRepository, MockDiscountRepository>();
        services.AddSingleton<ICashRegisterRepository, MockCashRegisterRepository>();
        services.AddSingleton<IPurchaseRepository, MockPurchaseRepository>();
        services.AddSingleton<ITaxRateRepository, MockTaxRateRepository>();

        // Application layer
        services.AddApplication();

        return services;
    }
}
