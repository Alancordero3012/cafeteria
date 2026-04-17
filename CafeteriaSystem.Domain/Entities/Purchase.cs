namespace CafeteriaSystem.Domain.Entities;

public enum PurchaseStatus { Pendiente, Recibida, Parcial, Cancelada }

public class Purchase
{
    public int Id { get; set; }                                     // id_compra
    public int? SupplierId { get; set; }                            // id_proveedor
    public Supplier? Supplier { get; set; }
    public int? UserId { get; set; }                                // id_usuario
    public User? User { get; set; }
    public string PurchaseNumber { get; set; } = string.Empty;     // numero_doc
    public DateTime Date { get; set; } = DateTime.Now;             // fecha
    public decimal Subtotal { get; set; }                          // subtotal
    public decimal TaxAmount { get; set; }                         // itbis
    public decimal Total { get; set; }                             // total
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Recibida; // estado

    // Notes no existe en el SQL (solo para UI, no mapeado):
    public string Notes { get; set; } = string.Empty;

    public ICollection<PurchaseDetail> Details { get; set; } = new List<PurchaseDetail>();
}
