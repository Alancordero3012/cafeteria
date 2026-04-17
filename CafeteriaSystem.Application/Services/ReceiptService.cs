using CafeteriaSystem.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CafeteriaSystem.Application.Services;

/// <summary>
/// Genera recibos de venta en formato PDF usando QuestPDF.
/// </summary>
public class ReceiptService
{
    // Info del negocio (se puede leer de BD en futuras versiones)
    private const string BusinessName    = "CafeteriaSystem";
    private const string BusinessPhone   = "(809) 000-0000";
    private const string BusinessAddress = "Santo Domingo, RD";

    /// <summary>Genera el PDF del recibo y lo devuelve como byte[].</summary>
    public byte[] GeneratePdf(Sale sale)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A6);          // Tamaño media hoja — ideal para recibos
                page.Margin(1, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                page.Content().Column(col =>
                {
                    col.Spacing(4);

                    // ── Header ─────────────────────────────────────────────
                    col.Item().AlignCenter().Text(BusinessName)
                        .Bold().FontSize(16);
                    col.Item().AlignCenter().Text(BusinessAddress)
                        .FontSize(8).FontColor(Colors.Grey.Medium);
                    col.Item().AlignCenter().Text($"Tel: {BusinessPhone}")
                        .FontSize(8).FontColor(Colors.Grey.Medium);

                    col.Item().PaddingVertical(4).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                    // ── Datos de la venta ───────────────────────────────────
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"Recibo #: {sale.SaleNumber}").SemiBold();
                        row.RelativeItem().AlignRight()
                           .Text(sale.SaleDate.ToString("dd/MM/yyyy HH:mm")).FontColor(Colors.Grey.Medium);
                    });

                    var customerName = sale.Customer?.Name 
                        ?? (string.IsNullOrEmpty(sale.Notes) ? "Consumidor Final" : sale.Notes);
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"Cliente: {customerName}");
                        row.RelativeItem().AlignRight()
                           .Text($"Cajero: {sale.User?.Username ?? "-"}").FontColor(Colors.Grey.Medium);
                    });

                    col.Item().PaddingVertical(4).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                    // ── Tabla de productos ──────────────────────────────────
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(4);   // Producto
                            cols.RelativeColumn(1);   // Cant
                            cols.RelativeColumn(2);   // Precio
                            cols.RelativeColumn(2);   // Total
                        });

                        // Encabezados
                        static IContainer HeaderCell(IContainer c) =>
                            c.DefaultTextStyle(x => x.SemiBold().FontSize(8))
                             .PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten1);

                        table.Header(h =>
                        {
                            h.Cell().Element(HeaderCell).Text("PRODUCTO");
                            h.Cell().Element(HeaderCell).AlignRight().Text("CANT");
                            h.Cell().Element(HeaderCell).AlignRight().Text("P.UNIT");
                            h.Cell().Element(HeaderCell).AlignRight().Text("TOTAL");
                        });

                        // Filas de detalle
                        foreach (var detail in sale.Details)
                        {
                            var name = detail.Product?.Name ?? "Producto";
                            table.Cell().PaddingVertical(2).Text(name);
                            table.Cell().PaddingVertical(2).AlignRight().Text(detail.Quantity.ToString("F0"));
                            table.Cell().PaddingVertical(2).AlignRight().Text($"{detail.UnitPrice:N2}");
                            table.Cell().PaddingVertical(2).AlignRight().Text($"{detail.Total:N2}");
                        }
                    });

                    col.Item().PaddingVertical(4).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                    // ── Totales ─────────────────────────────────────────────
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Subtotal:");
                        row.ConstantItem(80).AlignRight().Text($"RD$ {sale.Subtotal:N2}");
                    });

                    if (sale.DiscountAmount > 0)
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Descuento:").FontColor(Colors.Green.Medium);
                            row.ConstantItem(80).AlignRight().Text($"- RD$ {sale.DiscountAmount:N2}").FontColor(Colors.Green.Medium);
                        });
                    }

                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("ITBIS (18%):");
                        row.ConstantItem(80).AlignRight().Text($"RD$ {sale.TaxAmount:N2}");
                    });

                    col.Item().PaddingVertical(2).Row(row =>
                    {
                        row.RelativeItem().Text("TOTAL:").Bold().FontSize(11);
                        row.ConstantItem(80).AlignRight().Text($"RD$ {sale.Total:N2}").Bold().FontSize(11);
                    });

                    col.Item().PaddingVertical(1).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"Método: {sale.PaymentMethodEntity?.Name ?? "Efectivo"}").FontColor(Colors.Grey.Medium);
                    });
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Pagado:");
                        row.ConstantItem(80).AlignRight().Text($"RD$ {sale.AmountPaid:N2}");
                    });
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Cambio:");
                        row.ConstantItem(80).AlignRight().Text($"RD$ {sale.Change:N2}");
                    });

                    col.Item().PaddingVertical(6).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                    // ── Footer ──────────────────────────────────────────────
                    col.Item().AlignCenter().Text("¡Gracias por su compra!")
                        .Bold().FontSize(9);
                    col.Item().AlignCenter().Text("Conserve su recibo")
                        .FontSize(7).FontColor(Colors.Grey.Medium);
                });
            });
        }).GeneratePdf();
    }

    /// <summary>Guarda el PDF en la ruta indicada.</summary>
    public void SavePdf(Sale sale, string path)
    {
        var bytes = GeneratePdf(sale);
        File.WriteAllBytes(path, bytes);
    }

    /// <summary>Genera el PDF, lo guarda en temp y lo abre con el visor del sistema.</summary>
    public void OpenPdf(Sale sale)
    {
        var temp = Path.Combine(Path.GetTempPath(), $"Recibo_{sale.SaleNumber}.pdf");
        SavePdf(sale, temp);
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = temp,
            UseShellExecute = true
        });
    }
}
