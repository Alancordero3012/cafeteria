using System.Collections.ObjectModel;
using CafeteriaSystem.Application.Services;
using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace CafeteriaSystem.WPF.ViewModels;

public partial class InventoryViewModel(IProductRepository productRepository, ICategoryRepository categoryRepository) : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<Product> _products = [];
    [ObservableProperty] private ObservableCollection<Category> _categories = [];
    [ObservableProperty] private Category? _selectedCategory;
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private Product? _selectedProduct;
    [ObservableProperty] private bool _showLowStockOnly;
    private List<Product> _allProducts = [];

    public override async Task OnNavigatedToAsync()
    {
        IsBusy = true;
        _allProducts = (await productRepository.GetActivesAsync()).ToList();
        var cats = (await categoryRepository.GetAllAsync()).ToList();
        var allCat = new Category { Id = 0, Name = "Todas" };
        Categories = new ObservableCollection<Category>([allCat, .. cats]);
        SelectedCategory = allCat;
        Filter();
        IsBusy = false;
    }

    partial void OnSearchTextChanged(string value) => Filter();
    partial void OnSelectedCategoryChanged(Category? value) => Filter();
    partial void OnShowLowStockOnlyChanged(bool value) => Filter();

    private void Filter()
    {
        var q = _allProducts.AsEnumerable();
        if (SelectedCategory?.Id > 0) q = q.Where(p => p.CategoryId == SelectedCategory.Id);
        if (!string.IsNullOrWhiteSpace(SearchText)) q = q.Where(p => p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        if (ShowLowStockOnly) q = q.Where(p => p.HasLowStock);
        Products = new ObservableCollection<Product>(q.OrderBy(p => p.Name));
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        _allProducts = (await productRepository.GetActivesAsync()).ToList();
        Filter();
    }
}

public partial class SalesHistoryViewModel(ISaleRepository saleRepository) : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<Sale> _sales = [];
    [ObservableProperty] private DateTime _fromDate = DateTime.Today.AddDays(-30);
    [ObservableProperty] private DateTime _toDate = DateTime.Today;
    [ObservableProperty] private Sale? _selectedSale;
    [ObservableProperty] private decimal _periodTotal;

    public override async Task OnNavigatedToAsync() => await LoadAsync();

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        var list = (await saleRepository.GetByDateRangeAsync(FromDate, ToDate.AddDays(1))).OrderByDescending(s => s.SaleDate).ToList();
        Sales = new ObservableCollection<Sale>(list);
        PeriodTotal = list.Where(s => s.Status == SaleStatus.Completada).Sum(s => s.Total);
        IsBusy = false;
    }
}

public partial class ProductsViewModel(IProductRepository productRepository, ICategoryRepository categoryRepository) : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<Product> _products = [];
    [ObservableProperty] private ObservableCollection<Category> _categories = [];
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private Product? _selectedProduct;
    [ObservableProperty] private bool _isEditing;
    private List<Product> _allProducts = [];

    public override async Task OnNavigatedToAsync()
    {
        IsBusy = true;
        _allProducts = (await productRepository.GetAllAsync()).ToList();
        Categories = new ObservableCollection<Category>(await categoryRepository.GetAllAsync());
        Filter();
        IsBusy = false;
    }

    partial void OnSearchTextChanged(string v) => Filter();
    private void Filter()
    {
        var q = _allProducts.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText)) q = q.Where(p => p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        Products = new ObservableCollection<Product>(q.OrderBy(p => p.Name));
    }

    [RelayCommand] private void EditProduct(Product p) { SelectedProduct = p; IsEditing = true; }
    [RelayCommand] private void CancelEdit() { IsEditing = false; SelectedProduct = null; }

    [RelayCommand]
    private async Task SaveProductAsync()
    {
        if (SelectedProduct == null) return;
        if (SelectedProduct.Id == 0) await productRepository.AddAsync(SelectedProduct);
        else await productRepository.UpdateAsync(SelectedProduct);
        IsEditing = false;
        _allProducts = (await productRepository.GetAllAsync()).ToList();
        Filter();
    }
}

public partial class ReportsViewModel(ReportService reportService) : BaseViewModel
{
    [ObservableProperty] private object? _barSeries;
    [ObservableProperty] private object? _pieSeries;
    [ObservableProperty] private IEnumerable<LiveChartsCore.SkiaSharpView.Axis>? _barXAxes;
    [ObservableProperty] private IEnumerable<LiveChartsCore.SkiaSharpView.Axis>? _barYAxes;

    public override async Task OnNavigatedToAsync()
    {
        IsBusy = true;
        var data = await reportService.GetDashboardAsync();
        var labels = data.VentasPorDia.Select(v => v.Dia).ToArray();
        BarXAxes = new LiveChartsCore.SkiaSharpView.Axis[] { new() { Labels = labels, LabelsPaint = null, SeparatorsPaint = null, TicksPaint = null } };
        BarYAxes = new LiveChartsCore.SkiaSharpView.Axis[] { new() { LabelsPaint = null, SeparatorsPaint = null, TicksPaint = null } };
        BarSeries = new LiveChartsCore.SkiaSharpView.ColumnSeries<decimal>[]
        {
            new() { Values = data.VentasPorDia.Select(v => v.Total).ToArray(),
                    Fill = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SkiaSharp.SKColor.Parse("#E8A87C")),
                    Name = "RD$ Ventas" }
        };
        PieSeries = data.VentasPorCategoria.Select(c =>
            new LiveChartsCore.SkiaSharpView.PieSeries<decimal>
            {
                Values = new[] { c.Total }, Name = c.Categoria,
                Fill = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SkiaSharp.SKColor.Parse(c.Color))
            }).ToArray();
        IsBusy = false;
    }
}
