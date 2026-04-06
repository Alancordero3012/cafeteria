namespace CafeteriaSystem.Domain.Entities;

public enum DiscountType { Porcentaje, MontoFijo }
public enum DiscountScope { Todos, Categoria, Producto }

public class Discount
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DiscountType Type { get; set; } = DiscountType.Porcentaje;
    public decimal Value { get; set; }
    public DiscountScope Scope { get; set; } = DiscountScope.Todos;
    public int? CategoryId { get; set; }
    public int? ProductId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsValid => IsActive &&
        (StartDate == null || StartDate <= DateTime.Now) &&
        (EndDate == null || EndDate >= DateTime.Now);
}
