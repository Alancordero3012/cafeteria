namespace CafeteriaSystem.Application.DTOs;

public record CartItemDto
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TaxPercent { get; init; }
    public string ImageUrl { get; init; } = string.Empty;

    public decimal Subtotal => UnitPrice * Quantity;
    public decimal DiscountAmount => Subtotal * (DiscountPercent / 100);
    public decimal TaxAmount => (Subtotal - DiscountAmount) * (TaxPercent / 100);
    public decimal Total => Subtotal - DiscountAmount + TaxAmount;
}
