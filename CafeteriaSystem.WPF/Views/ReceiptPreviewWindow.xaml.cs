using System.Windows;
using CafeteriaSystem.Application.Services;
using CafeteriaSystem.Domain.Entities;
using Microsoft.Win32;

namespace CafeteriaSystem.WPF.Views;

public partial class ReceiptPreviewWindow : Window
{
    private readonly Sale _sale;
    private readonly ReceiptService _receiptService;

    public ReceiptPreviewWindow(Sale sale, ReceiptService receiptService)
    {
        InitializeComponent();
        _sale = sale;
        _receiptService = receiptService;
        PopulateReceipt();
    }

    private void PopulateReceipt()
    {
        var customerName = _sale.Customer?.Name 
                        ?? (string.IsNullOrEmpty(_sale.Notes) ? "Consumidor Final" : _sale.Notes);
        var cashier = _sale.User?.Username ?? "-";

        TxtSaleNumber.Text = $"Recibo #: {_sale.SaleNumber}";
        TxtDate.Text       = _sale.SaleDate.ToString("dd/MM/yyyy HH:mm");
        TxtCustomer.Text   = $"Cliente: {customerName}";
        TxtCashier.Text    = $"Cajero: {cashier}";
        TxtSubtotal.Text   = $"RD$ {_sale.Subtotal:N2}";
        TxtTax.Text        = $"RD$ {_sale.TaxAmount:N2}";
        TxtTotal.Text      = $"RD$ {_sale.Total:N2}";
        TxtPaid.Text       = $"RD$ {_sale.AmountPaid:N2}";
        TxtChange.Text     = $"RD$ {_sale.Change:N2}";
        TxtMethod.Text     = $"Método: {_sale.PaymentMethodEntity?.Name ?? "Efectivo"}";

        if (_sale.DiscountAmount > 0)
        {
            DiscountRow.Visibility = Visibility.Visible;
            TxtDiscount.Text = $"- RD$ {_sale.DiscountAmount:N2}";
        }

        // Poblar items con ProductName resuelto desde la navegación
        var items = _sale.Details.Select(d => new
        {
            ProductName = d.Product?.Name ?? d.ProductName,
            d.Quantity,
            d.UnitPrice,
            d.Total
        }).ToList();

        ItemsList.ItemsSource = items;
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

    private void BtnPrint_Click(object sender, RoutedEventArgs e)
    {
        try { _receiptService.OpenPdf(_sale); }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al abrir el PDF: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnSavePdf_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title      = "Guardar Recibo",
            FileName   = $"Recibo_{_sale.SaleNumber}",
            DefaultExt = ".pdf",
            Filter     = "PDF Files (*.pdf)|*.pdf"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                _receiptService.SavePdf(_sale, dialog.FileName);
                MessageBox.Show($"Recibo guardado correctamente.", "Guardado",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
