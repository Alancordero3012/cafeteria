namespace CafeteriaSystem.Domain.Entities;

public class SaleDetail
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public Sale? Sale { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal Subtotal => UnitPrice * Quantity;
    public decimal DiscountAmount => Subtotal * (DiscountPercent / 100);
    public decimal TaxAmount => (Subtotal - DiscountAmount) * (TaxPercent / 100);
    public decimal Total => Subtotal - DiscountAmount + TaxAmount;
}
