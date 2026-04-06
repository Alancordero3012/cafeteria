namespace CafeteriaSystem.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "Cliente General";
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string RNC { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public decimal TotalPurchases { get; set; }
    public int TotalOrders { get; set; }
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
