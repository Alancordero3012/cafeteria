namespace CafeteriaSystem.Domain.Entities;

public class ActivityLog
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string Action { get; set; } = string.Empty;   // LOGIN, VENTA, PRODUCTO_CREADO, etc.
    public string Module { get; set; } = string.Empty;   // Auth, POS, Productos
    public string Details { get; set; } = string.Empty;  // Contexto libre
    public DateTime Date { get; set; } = DateTime.Now;

    // Navegación (opcional, para reportes)
    public User? User { get; set; }
}
