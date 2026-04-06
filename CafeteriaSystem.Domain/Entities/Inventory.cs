namespace CafeteriaSystem.Domain.Entities;

public class Inventory
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public string Location { get; set; } = "Almacén Principal";
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}
