using CafeteriaSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CafeteriaSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<SaleService>();
        services.AddScoped<ReportService>();
        return services;
    }
}
