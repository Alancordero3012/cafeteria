namespace CafeteriaSystem.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool CanManageProducts { get; set; }
    public bool CanManageUsers { get; set; }
    public bool CanViewReports { get; set; }
    public bool CanManagePurchases { get; set; }
    public bool CanOpenCloseCash { get; set; }
    public bool CanApplyDiscounts { get; set; }
    public bool CanCancelSales { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}
