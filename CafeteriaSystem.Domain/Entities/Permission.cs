namespace CafeteriaSystem.Domain.Entities;

/// <summary>
/// Mapea la tabla Permisos del esquema sistema_ventasbd.
/// Cada permiso define qué puede hacer un rol dentro de un módulo.
/// </summary>
public class Permission
{
    public int Id { get; set; }        // id_permiso
    public int RoleId { get; set; }    // id_rol
    public Role? Role { get; set; }
    public string Module { get; set; } = string.Empty;   // modulo
    public bool CanView { get; set; }                    // puede_ver
    public bool CanCreate { get; set; }                  // puede_crear
    public bool CanEdit { get; set; }                    // puede_editar
    public bool CanDelete { get; set; }                  // puede_eliminar
}
