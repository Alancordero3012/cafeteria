namespace CafeteriaSystem.Domain.Entities;

public class User
{
    public int Id { get; set; }                                     // id_usuario
    public int? RoleId { get; set; }                                // id_rol
    public Role? Role { get; set; }
    public string FullName { get; set; } = string.Empty;            // nombre
    public string Username { get; set; } = string.Empty;            // usuario
    public string PasswordHash { get; set; } = string.Empty;        // password_hash
    public string Email { get; set; } = string.Empty;               // email
    public bool IsActive { get; set; } = true;                      // activo
    public DateTime? LastLogin { get; set; }                        // ultimo_acceso
    public DateTime CreatedAt { get; set; } = DateTime.Now;         // creado_en

    // Propiedad visual: no se persiste en BD, se calcula en memoria
    public string AvatarColor { get; set; } = "#E8A87C";

    public string Initials => FullName.Length > 0
        ? string.Join("", FullName.Split(' ').Take(2).Select(w => w[0]))
        : "?";

    // Compatibilidad: Phone no existe en el esquema SQL
    public string Phone { get; set; } = string.Empty;
}
