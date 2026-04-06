namespace CafeteriaSystem.Domain.Entities;

public enum CashRegisterStatus { Abierta, Cerrada }

public class CashRegister
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime OpenDate { get; set; } = DateTime.Now;
    public DateTime? CloseDate { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal ExpectedAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public decimal Difference => ActualAmount - ExpectedAmount;
    public CashRegisterStatus Status { get; set; } = CashRegisterStatus.Abierta;
    public string Notes { get; set; } = string.Empty;
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
