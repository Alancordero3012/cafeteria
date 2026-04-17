using System.Collections.ObjectModel;
using CafeteriaSystem.Application.DTOs;
using CafeteriaSystem.Application.Services;
using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CafeteriaSystem.WPF.ViewModels;

public partial class POSViewModel(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    ICustomerRepository customerRepository,
    SaleService saleService,
    AuthService authService) : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<Product> _displayedProducts = [];
    [ObservableProperty] private ObservableCollection<Category> _categories = [];
    [ObservableProperty] private ObservableCollection<CartItemDto> _cartItems = [];
    [ObservableProperty] private ObservableCollection<Customer> _customers = [];
    [ObservableProperty] private Category? _selectedCategory;
    [ObservableProperty] private Customer? _selectedCustomer;
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private PaymentMethod _selectedPaymentMethod = PaymentMethod.Efectivo;
    [ObservableProperty] private decimal _amountPaid;
    [ObservableProperty] private decimal _manualDiscountPercent;
    // Totals
    [ObservableProperty] private decimal _subtotal;
    [ObservableProperty] private decimal _taxAmount;
    [ObservableProperty] private decimal _discountAmount;
    [ObservableProperty] private decimal _total;
    [ObservableProperty] private decimal _change;
    [ObservableProperty] private int _cartItemCount;
    [ObservableProperty] private bool _saleSuccess;
    [ObservableProperty] private string _lastSaleNumber = string.Empty;
    [ObservableProperty] private bool _insufficientAmount;
    [ObservableProperty] private decimal _shortage;
    [ObservableProperty] private string _customerName = string.Empty;

    private List<Product> _allProducts = [];

    public override async Task OnNavigatedToAsync()
    {
        IsBusy = true;
        _allProducts = (await productRepository.GetActivesAsync()).ToList();
        var cats = (await categoryRepository.GetAllAsync()).ToList();
        var allCat = new Category { Id = 0, Name = "Todos", Color = "#E8A87C", Icon = "ViewGrid" };
        Categories = new ObservableCollection<Category>([allCat, .. cats]);
        SelectedCategory = allCat;
        Customers = new ObservableCollection<Customer>(await customerRepository.GetAllAsync());
        SelectedCustomer = Customers.FirstOrDefault();
        FilterProducts();
        IsBusy = false;
    }

    [RelayCommand]
    private void SelectCategory(Category category)
    {
        SelectedCategory = category;
    }

    partial void OnSearchTextChanged(string value) => FilterProducts();
    partial void OnSelectedCategoryChanged(Category? value) => FilterProducts();

    private void FilterProducts()
    {
        var filtered = _allProducts.AsEnumerable();
        if (SelectedCategory?.Id > 0)
            filtered = filtered.Where(p => p.CategoryId == SelectedCategory.Id);
        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(p => p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        DisplayedProducts = new ObservableCollection<Product>(filtered);
    }

    [RelayCommand]
    private void AddToCart(Product product)
    {
        if (product.CurrentStock <= 0) { StatusMessage = $"Sin stock: {product.Name}"; return; }
        var existing = CartItems.FirstOrDefault(i => i.ProductId == product.Id);
        if (existing != null)
        {
            var currentInCart = CartItems.IndexOf(existing);
            if (existing.Quantity >= product.CurrentStock) { StatusMessage = "Stock máximo en carrito."; return; }
            CartItems[currentInCart] = existing with { Quantity = existing.Quantity + 1 };
        }
        else
        {
            CartItems.Add(new CartItemDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                CategoryName = product.Category?.Name ?? "",
                UnitPrice = product.Price,
                Quantity = 1,
                TaxPercent = product.AplicaItbis ? 18m : 0m,
            });
        }
        CalculateTotals();
    }

    [RelayCommand]
    private void IncreaseQty(CartItemDto item)
    {
        var product = _allProducts.FirstOrDefault(p => p.Id == item.ProductId);
        if (product == null || item.Quantity >= product.CurrentStock) return;
        var idx = CartItems.IndexOf(item);
        if (idx >= 0) CartItems[idx] = item with { Quantity = item.Quantity + 1 };
        CalculateTotals();
    }

    [RelayCommand]
    private void DecreaseQty(CartItemDto item)
    {
        var idx = CartItems.IndexOf(item);
        if (item.Quantity <= 1) CartItems.RemoveAt(idx);
        else if (idx >= 0) CartItems[idx] = item with { Quantity = item.Quantity - 1 };
        CalculateTotals();
    }

    [RelayCommand]
    private void RemoveItem(CartItemDto item) { CartItems.Remove(item); CalculateTotals(); }

    [RelayCommand]
    private void ClearCart() { CartItems.Clear(); CalculateTotals(); }

    partial void OnAmountPaidChanged(decimal value)
    {
        Change = value - Total;
        UpdateInsufficient();
    }
    partial void OnManualDiscountPercentChanged(decimal value) => CalculateTotals();

    private void CalculateTotals()
    {
        Subtotal = CartItems.Sum(i => i.Subtotal);
        var itemDiscounts = CartItems.Sum(i => i.DiscountAmount);
        var manual = Subtotal * (ManualDiscountPercent / 100);
        DiscountAmount = itemDiscounts + manual;
        TaxAmount = CartItems.Sum(i => i.TaxAmount);
        Total = Subtotal - DiscountAmount + TaxAmount;
        Change = AmountPaid - Total;
        CartItemCount = CartItems.Sum(i => i.Quantity);
        UpdateInsufficient();
    }

    private void UpdateInsufficient()
    {
        Shortage = Total - AmountPaid;
        InsufficientAmount = CartItems.Count > 0 && AmountPaid > 0 && AmountPaid < Total;
    }

    [RelayCommand]
    private async Task ProcessSaleAsync()
    {
        if (CartItems.Count == 0) { StatusMessage = "El carrito está vacío."; return; }
        if (SelectedPaymentMethod == PaymentMethod.Efectivo && AmountPaid < Total)
        { StatusMessage = $"Monto insuficiente. Falta: RD${Total - AmountPaid:N2}"; return; }

        IsBusy = true;
        var session = authService.CurrentSession;
        var result = await saleService.ProcessSaleAsync(
            CartItems.ToList(),
            session?.Id ?? 1,
            SelectedCustomer?.Id,
            SelectedPaymentMethod,
            AmountPaid,
            paymentMethodId: null,
            ManualDiscountPercent,
            customerName: string.IsNullOrWhiteSpace(CustomerName) ? null : CustomerName);
        IsBusy = false;

        if (!result.Success) { StatusMessage = result.Message; return; }

        LastSaleNumber = result.Sale!.SaleNumber;
        SaleSuccess = true;
        CartItems.Clear();
        AmountPaid = 0;
        ManualDiscountPercent = 0;
        CustomerName = string.Empty;
        CalculateTotals();
        // Refresh stock display
        _allProducts = (await productRepository.GetActivesAsync()).ToList();
        FilterProducts();
        await Task.Delay(3000);
        SaleSuccess = false;
    }
}
