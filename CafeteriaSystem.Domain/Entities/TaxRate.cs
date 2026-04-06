namespace CafeteriaSystem.Domain.Entities;

public class TaxRate
{
    public int Id { get; set; }
    public string Name { get; set; } = "ITBIS";
    public decimal Rate { get; set; } = 0.18m;
    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;
}
