namespace CafeteriaSystem.Application.DTOs;

public record UserSessionDto
{
    public int Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string AvatarColor { get; init; } = "#E8A87C";
    public string Initials { get; init; } = string.Empty;
    public bool CanManageProducts { get; init; }
    public bool CanManageUsers { get; init; }
    public bool CanViewReports { get; init; }
    public bool CanManagePurchases { get; init; }
    public bool CanOpenCloseCash { get; init; }
    public bool CanApplyDiscounts { get; init; }
    public bool CanCancelSales { get; init; }
}
