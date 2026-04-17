namespace CafeteriaSystem.Domain.Entities;

public class PurchaseDetail
{
    public int Id { get; set; }                                  // id_detalle
    public int PurchaseId { get; set; }                          // id_compra
    public Purchase? Purchase { get; set; }
    public int ProductId { get; set; }                           // id_producto
    public Product? Product { get; set; }
    public decimal Quantity { get; set; }                        // cantidad (decimal en BD)
    public decimal UnitCost { get; set; }                        // precio_unitario
    public decimal TaxAmount { get; set; }                       // itbis
    public decimal Subtotal { get; set; }                        // subtotal (persistido en BD)

    // Propiedad de UI, no persistida
    public string ProductName { get; set; } = string.Empty;
}
