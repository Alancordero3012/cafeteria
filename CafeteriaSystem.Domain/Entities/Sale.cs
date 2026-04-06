namespace CafeteriaSystem.Domain.Entities;

public enum SaleStatus { Pendiente, Completada, Cancelada, Devuelta }
public enum PaymentMethod { Efectivo, Tarjeta, Transferencia, Mixto }

public class Sale
{
    public int Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int? CashRegisterId { get; set; }
    public CashRegister? CashRegister { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Change { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;
    public SaleStatus Status { get; set; } = SaleStatus.Completada;
    public string Notes { get; set; } = string.Empty;
    public ICollection<SaleDetail> Details { get; set; } = new List<SaleDetail>();
}
