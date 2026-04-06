namespace CafeteriaSystem.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLogin { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string AvatarColor { get; set; } = "#E8A87C";

    public string Initials => FullName.Length > 0
        ? string.Join("", FullName.Split(' ').Take(2).Select(w => w[0]))
        : "?";
}
