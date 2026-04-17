namespace CafeteriaSystem.Domain.Entities;

public class Role
{
    public int Id { get; set; }                                        // id_rol
    public string Name { get; set; } = string.Empty;                  // nombre
    public string Description { get; set; } = string.Empty;           // descripcion

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public ICollection<User> Users { get; set; } = new List<User>();

    // ── Permisos calculados desde la tabla Permisos ─────────────────────────
    // Se calculan en el repositorio a partir de los registros de Permisos.
    // Permiten mantener compatibilidad con el AuthService sin cambios.
    public bool CanManageProducts => HasPermission("Productos");
    public bool CanManageUsers => HasPermission("Usuarios");
    public bool CanViewReports => HasViewPermission("Reportes");
    public bool CanManagePurchases => HasPermission("Compras");
    public bool CanOpenCloseCash => HasPermission("Caja");
    public bool CanApplyDiscounts => HasPermission("Descuentos");
    public bool CanCancelSales => HasPermission("Ventas");

    private bool HasPermission(string module) =>
        Permissions.Any(p => p.Module == module && (p.CanCreate || p.CanEdit || p.CanDelete));

    private bool HasViewPermission(string module) =>
        Permissions.Any(p => p.Module == module && p.CanView);
}
