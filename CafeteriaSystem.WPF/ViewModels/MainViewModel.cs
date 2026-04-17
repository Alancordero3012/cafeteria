using CafeteriaSystem.Application.DTOs;
using CafeteriaSystem.Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace CafeteriaSystem.WPF.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AuthService _authService;

    [ObservableProperty] private BaseViewModel? _currentViewModel;
    [ObservableProperty] private bool _isLoggedIn;
    [ObservableProperty] private UserSessionDto? _currentUser;
    [ObservableProperty] private LoginViewModel? _loginViewModel;
    [ObservableProperty] private string _activeSection = "Dashboard";

    // Nav item selection
    [ObservableProperty] private bool _isDashboardActive = true;
    [ObservableProperty] private bool _isPOSActive;
    [ObservableProperty] private bool _isInventoryActive;
    [ObservableProperty] private bool _isSalesActive;
    [ObservableProperty] private bool _isReportsActive;
    [ObservableProperty] private bool _isProductsActive;
    [ObservableProperty] private bool _isCategoriesActive;

    public MainViewModel(IServiceProvider serviceProvider, AuthService authService, LoginViewModel loginViewModel)
    {
        _serviceProvider = serviceProvider;
        _authService = authService;
        _loginViewModel = loginViewModel;
        _loginViewModel.LoginSucceeded += OnLoginSucceeded;
    }

    private void OnLoginSucceeded(UserSessionDto session)
    {
        CurrentUser = session;
        IsLoggedIn = true;
        NavigateToDashboard();
    }

    private void ClearActiveState() { IsDashboardActive = IsPOSActive = IsInventoryActive = IsSalesActive = IsReportsActive = IsProductsActive = IsCategoriesActive = false; }

    [RelayCommand]
    private async Task NavigateToDashboard()
    {
        ClearActiveState(); IsDashboardActive = true; ActiveSection = "Dashboard";
        var vm = _serviceProvider.GetRequiredService<DashboardViewModel>();
        await vm.OnNavigatedToAsync();
        CurrentViewModel = vm;
    }

    [RelayCommand]
    private async Task NavigateToPOS()
    {
        ClearActiveState(); IsPOSActive = true; ActiveSection = "Punto de Venta";
        var vm = _serviceProvider.GetRequiredService<POSViewModel>();
        await vm.OnNavigatedToAsync();
        CurrentViewModel = vm;
    }

    [RelayCommand]
    private async Task NavigateToInventory()
    {
        ClearActiveState(); IsInventoryActive = true; ActiveSection = "Inventario";
        var vm = _serviceProvider.GetRequiredService<InventoryViewModel>();
        await vm.OnNavigatedToAsync();
        CurrentViewModel = vm;
    }

    [RelayCommand]
    private async Task NavigateToSales()
    {
        ClearActiveState(); IsSalesActive = true; ActiveSection = "Historial de Ventas";
        var vm = _serviceProvider.GetRequiredService<SalesHistoryViewModel>();
        await vm.OnNavigatedToAsync();
        CurrentViewModel = vm;
    }

    [RelayCommand]
    private async Task NavigateToReports()
    {
        ClearActiveState(); IsReportsActive = true; ActiveSection = "Reportes";
        var vm = _serviceProvider.GetRequiredService<ReportsViewModel>();
        await vm.OnNavigatedToAsync();
        CurrentViewModel = vm;
    }

    [RelayCommand]
    private async Task NavigateToProducts()
    {
        ClearActiveState(); IsProductsActive = true; ActiveSection = "Productos";
        var vm = _serviceProvider.GetRequiredService<ProductsViewModel>();
        await vm.OnNavigatedToAsync();
        CurrentViewModel = vm;
    }

    [RelayCommand]
    private async Task NavigateToCategories()
    {
        ClearActiveState(); IsCategoriesActive = true; ActiveSection = "Categorías";
        var vm = _serviceProvider.GetRequiredService<CategoriesViewModel>();
        await vm.OnNavigatedToAsync();
        CurrentViewModel = vm;
    }

    [RelayCommand]
    private void Logout()
    {
        _authService.Logout();
        IsLoggedIn = false;
        CurrentUser = null;
        CurrentViewModel = null;
    }
}
