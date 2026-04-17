namespace CafeteriaSystem.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;   // código en el SQL
    public decimal Price { get; set; }                    // precio_venta
    public decimal Cost { get; set; }                     // precio_compra
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int? SupplierId { get; set; }                  // id_proveedor (nuevo)
    public Supplier? Supplier { get; set; }
    public bool AplicaItbis { get; set; } = true;         // aplica_itbis
    public bool IsActive { get; set; } = true;
    public string ImageUrl { get; set; } = string.Empty;  // no mapeado a BD
    public string Unit { get; set; } = "Und";             // unidad_medida
    public decimal MinimumStock { get; set; } = 5;        // stock_minimo
    public decimal CurrentStock { get; set; }             // stock_actual
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }              // actualizado_en

    public bool HasLowStock => CurrentStock <= MinimumStock;

    // Compatibilidad UI: el precio de venta con/sin ITBIS
    public decimal PriceWithTax => AplicaItbis ? Price * 1.18m : Price;
}
