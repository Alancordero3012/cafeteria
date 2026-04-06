namespace CafeteriaSystem.Domain.Entities;

public enum PurchaseStatus { Pendiente, Recibida, Parcial, Cancelada }

public class Purchase
{
    public int Id { get; set; }
    public string PurchaseNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Total { get; set; }
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Recibida;
    public string Notes { get; set; } = string.Empty;
    public ICollection<PurchaseDetail> Details { get; set; } = new List<PurchaseDetail>();
}
