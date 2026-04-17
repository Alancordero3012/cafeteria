namespace CafeteriaSystem.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Propiedades de UI (no persistidas en BD):
    public string Color { get; set; } = "#E8A87C";
    public string Icon { get; set; } = "Coffee";

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
