namespace CafeteriaSystem.Domain.Entities;

public enum CashRegisterStatus { Abierta, Cerrada }

public class CashRegister
{
    public int Id { get; set; }                                    // id_arqueo
    public int? UserId { get; set; }                               // id_usuario
    public User? User { get; set; }
    public DateTime OpenDate { get; set; } = DateTime.Now;        // fecha_apertura
    public DateTime? CloseDate { get; set; }                       // fecha_cierre
    public decimal InitialAmount { get; set; }                    // monto_apertura
    public decimal? ActualAmount { get; set; }                    // monto_real
    public CashRegisterStatus Status { get; set; } = CashRegisterStatus.Abierta; // estado

    // No existen en SQL (se mantienen para compatibilidad UI, no mapeados):
    public decimal ExpectedAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public decimal Difference => (ActualAmount ?? 0) - ExpectedAmount;

    // La relación con Sales ya no va en Arqueos_Caja en este esquema
    // public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
