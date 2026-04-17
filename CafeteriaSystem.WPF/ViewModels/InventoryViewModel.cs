using System.Collections.ObjectModel;
using CafeteriaSystem.Application.Services;
using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CafeteriaSystem.WPF.Views;
using Microsoft.Win32;


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

public partial class SalesHistoryViewModel(
    ISaleRepository saleRepository,
    ReceiptService receiptService) : BaseViewModel
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
        var list = (await saleRepository.GetByDateRangeAsync(FromDate, ToDate.AddDays(1)))
            .OrderByDescending(s => s.SaleDate).ToList();
        Sales = new ObservableCollection<Sale>(list);
        PeriodTotal = list.Where(s => s.Status == SaleStatus.Completada).Sum(s => s.Total);
        IsBusy = false;
    }

    [RelayCommand]
    private void ViewReceipt(Sale sale)
    {
        if (sale == null) return;
        var window = new ReceiptPreviewWindow(sale, receiptService)
        {
            Owner = System.Windows.Application.Current.MainWindow
        };
        window.ShowDialog();
    }

    [RelayCommand]
    private void SaveReceipt(Sale sale)
    {
        if (sale == null) return;
        var dialog = new SaveFileDialog
        {
            Title      = "Guardar Recibo",
            FileName   = $"Recibo_{sale.SaleNumber}",
            DefaultExt = ".pdf",
            Filter     = "PDF Files (*.pdf)|*.pdf"
        };
        if (dialog.ShowDialog() == true)
        {
            try
            {
                receiptService.SavePdf(sale, dialog.FileName);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar: {ex.Message}";
            }
        }
    }
}

public partial class ProductsViewModel(IProductRepository productRepository, ICategoryRepository categoryRepository, ISupplierRepository supplierRepository) : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<Product> _products = [];
    [ObservableProperty] private ObservableCollection<Category> _categories = [];
    [ObservableProperty] private ObservableCollection<Domain.Entities.Supplier> _suppliers = [];
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private Product? _selectedProduct;
    [ObservableProperty] private bool _isEditing;
    private List<Product> _allProducts = [];

    // Campos del formulario de edición
    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editDescription = string.Empty;
    [ObservableProperty] private string _editBarcode = string.Empty;
    [ObservableProperty] private string _editUnit = "unidad";
    [ObservableProperty] private decimal _editPrice;
    [ObservableProperty] private decimal _editCost;
    [ObservableProperty] private decimal _editStock;
    [ObservableProperty] private decimal _editMinStock;
    [ObservableProperty] private bool _editAplicaItbis = true;
    [ObservableProperty] private Category? _editCategory;
    [ObservableProperty] private Domain.Entities.Supplier? _editSupplier;
    [ObservableProperty] private bool _editIsActive = true;
    [ObservableProperty] private string _formTitle = "Nuevo Producto";

    public override async Task OnNavigatedToAsync()
    {
        IsBusy = true;
        _allProducts = (await productRepository.GetAllAsync()).ToList();
        Categories = new ObservableCollection<Category>(await categoryRepository.GetAllAsync());
        Suppliers = new ObservableCollection<Domain.Entities.Supplier>(await supplierRepository.GetAllAsync());
        Filter();
        IsBusy = false;
    }

    partial void OnSearchTextChanged(string value) => Filter();
    private void Filter()
    {
        var q = _allProducts.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText)) q = q.Where(p => p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        Products = new ObservableCollection<Product>(q.OrderBy(p => p.Name));
    }

    [RelayCommand]
    private void NewProduct()
    {
        SelectedProduct = new Product();
        FormTitle = "Nuevo Producto";
        EditName = string.Empty;
        EditDescription = string.Empty;
        EditBarcode = string.Empty;
        EditUnit = "unidad";
        EditPrice = 0;
        EditCost = 0;
        EditStock = 0;
        EditMinStock = 0;
        EditAplicaItbis = true;
        EditIsActive = true;
        EditCategory = Categories.FirstOrDefault();
        EditSupplier = null;
        IsEditing = true;
    }

    [RelayCommand]
    private void EditProduct(Product p)
    {
        SelectedProduct = p;
        FormTitle = "Editar Producto";
        EditName = p.Name;
        EditDescription = p.Description;
        EditBarcode = p.Barcode;
        EditUnit = p.Unit;
        EditPrice = p.Price;
        EditCost = p.Cost;
        EditStock = p.CurrentStock;
        EditMinStock = p.MinimumStock;
        EditAplicaItbis = p.AplicaItbis;
        EditIsActive = p.IsActive;
        EditCategory = Categories.FirstOrDefault(c => c.Id == p.CategoryId);
        EditSupplier = Suppliers.FirstOrDefault(s => s.Id == p.SupplierId);
        IsEditing = true;
    }

    [RelayCommand]
    private void CancelEdit() { IsEditing = false; SelectedProduct = null; }

    [RelayCommand]
    private async Task SaveProductAsync()
    {
        if (string.IsNullOrWhiteSpace(EditName)) { StatusMessage = "El nombre es requerido."; return; }

        var product = SelectedProduct ?? new Product();
        product.Name = EditName;
        product.Description = EditDescription;
        product.Barcode = EditBarcode;
        product.Unit = EditUnit;
        product.Price = EditPrice;
        product.Cost = EditCost;
        product.CurrentStock = EditStock;
        product.MinimumStock = EditMinStock;
        product.AplicaItbis = EditAplicaItbis;
        product.IsActive = EditIsActive;
        product.CategoryId = EditCategory?.Id ?? 0;
        product.SupplierId = EditSupplier?.Id;   // int? → ok

        IsBusy = true;
        if (product.Id == 0) await productRepository.AddAsync(product);
        else await productRepository.UpdateAsync(product);
        IsBusy = false;

        StatusMessage = product.Id == 0 ? "Producto creado." : "Producto actualizado.";
        IsEditing = false;
        SelectedProduct = null;
        _allProducts = (await productRepository.GetAllAsync()).ToList();
        Filter();
    }

    [RelayCommand]
    private async Task DeleteProductAsync(Product p)
    {
        p.IsActive = false;
        await productRepository.UpdateAsync(p);
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
