namespace CafeteriaSystem.Domain.Entities;

/// <summary>
/// Mapea la tabla Metodos_Pago del esquema sistema_ventasbd.
/// (No confundir con el enum PaymentMethod usado en la UI)
/// </summary>
public class PaymentMethodEntity
{
    public int Id { get; set; }                    // id_metodo
    public string Name { get; set; } = string.Empty; // nombre
    public bool IsActive { get; set; } = true;     // activo

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
