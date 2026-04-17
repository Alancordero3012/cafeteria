namespace CafeteriaSystem.Domain.Entities;

public class SaleDetail
{
    public int Id { get; set; }                                  // id_detalle
    public int SaleId { get; set; }                              // id_venta
    public Sale? Sale { get; set; }
    public int ProductId { get; set; }                           // id_producto
    public Product? Product { get; set; }
    public decimal Quantity { get; set; }                        // cantidad (decimal en BD)
    public decimal UnitPrice { get; set; }                       // precio_unitario
    public decimal Discount { get; set; }                        // descuento (monto fijo, no %)
    public decimal TaxAmount { get; set; }                       // itbis
    public decimal Subtotal { get; set; }                        // subtotal (persistido en BD)

    // Propiedades de UI, no persistidas
    public string ProductName { get; set; } = string.Empty;
    public decimal Total => Subtotal - Discount + TaxAmount;
}
