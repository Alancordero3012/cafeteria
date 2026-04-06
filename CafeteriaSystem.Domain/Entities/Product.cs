namespace CafeteriaSystem.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int TaxRateId { get; set; }
    public TaxRate? TaxRate { get; set; }
    public bool IsActive { get; set; } = true;
    public string ImageUrl { get; set; } = string.Empty;
    public int MinimumStock { get; set; } = 5;
    public string Unit { get; set; } = "Und";
    public int CurrentStock { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public bool HasLowStock => CurrentStock <= MinimumStock;
}
