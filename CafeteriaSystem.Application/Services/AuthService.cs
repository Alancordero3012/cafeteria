using CafeteriaSystem.Application.DTOs;
using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;

namespace CafeteriaSystem.Application.Services;

public class AuthService(IUserRepository userRepository)
{
    private readonly IUserRepository _users = userRepository;
    private UserSessionDto? _currentSession;

    public UserSessionDto? CurrentSession => _currentSession;
    public bool IsLoggedIn => _currentSession != null;

    public async Task<(bool Success, string Message, UserSessionDto? Session)> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return (false, "Usuario y contraseña son requeridos.", null);

        var hash = HashPassword(password);
        var user = await _users.GetByUsernameAsync(username);
        if (user == null || !user.IsActive)
            return (false, "Usuario no encontrado o desactivado.", null);

        if (user.PasswordHash != hash)
            return (false, "Contraseña incorrecta.", null);

        _currentSession = MapToSession(user);
        return (true, "Bienvenido", _currentSession);
    }

    public void Logout() => _currentSession = null;

    public static string HashPassword(string password)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        return Convert.ToHexString(sha.ComputeHash(bytes)).ToLower();
    }

    private static UserSessionDto MapToSession(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        FullName = user.FullName,
        RoleName = user.Role?.Name ?? "Sin Rol",
        AvatarColor = user.AvatarColor,
        Initials = user.Initials,
        CanManageProducts = user.Role?.CanManageProducts ?? false,
        CanManageUsers = user.Role?.CanManageUsers ?? false,
        CanViewReports = user.Role?.CanViewReports ?? false,
        CanManagePurchases = user.Role?.CanManagePurchases ?? false,
        CanOpenCloseCash = user.Role?.CanOpenCloseCash ?? false,
        CanApplyDiscounts = user.Role?.CanApplyDiscounts ?? false,
        CanCancelSales = user.Role?.CanCancelSales ?? false,
    };
}
