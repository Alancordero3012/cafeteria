using CafeteriaSystem.Application.Services;
using CafeteriaSystem.Infrastructure;
using CafeteriaSystem.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestPDF.Infrastructure;
using System.Windows;

namespace CafeteriaSystem.WPF;

public partial class App : System.Windows.Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((ctx, services) =>
            {
                // Infrastructure + SQL Server (includes Application via AddApplication())
                services.AddInfrastructure(ctx.Configuration);

                // Auth as singleton so session persists
                services.AddSingleton<AuthService>();
                services.AddScoped<ActivityLogService>();
                services.AddScoped<ReceiptService>();

                // ViewModels
                services.AddSingleton<LoginViewModel>();
                services.AddSingleton<MainViewModel>();
                services.AddTransient<DashboardViewModel>();
                services.AddTransient<POSViewModel>();
                services.AddTransient<InventoryViewModel>();
                services.AddTransient<SalesHistoryViewModel>();
                services.AddTransient<ReportsViewModel>();
                services.AddTransient<ProductsViewModel>();
                services.AddTransient<CategoriesViewModel>();

                // Main window
                services.AddSingleton<MainWindow>();
            })
            .Build();

        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = _host.Services.GetRequiredService<MainViewModel>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
        base.OnExit(e);
    }
}
