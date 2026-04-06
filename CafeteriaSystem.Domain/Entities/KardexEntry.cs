namespace CafeteriaSystem.Domain.Entities;

public enum KardexType { Entrada, Salida, Ajuste, DevolucionVenta, DevolucionCompra }

public class KardexEntry
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public KardexType Type { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public int BalanceAfter { get; set; }
    public int UserId { get; set; }
}
