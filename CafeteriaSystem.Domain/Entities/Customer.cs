namespace CafeteriaSystem.Domain.Entities;

public class Customer
{
    public int Id { get; set; }                                   // id_cliente
    public string Cedula { get; set; } = string.Empty;           // cedula_rnc
    public string Name { get; set; } = "Cliente General";        // nombre
    public string Phone { get; set; } = string.Empty;            // telefono
    public string Email { get; set; } = string.Empty;            // email
    public string Address { get; set; } = string.Empty;          // direccion
    public decimal CreditLimit { get; set; } = 0;                // limite_credito
    public decimal PendingBalance { get; set; } = 0;             // saldo_pendiente
    public bool IsActive { get; set; } = true;                   // activo
    public DateTime CreatedAt { get; set; } = DateTime.Now;      // creado_en

    // RNC es alias de Cedula (compatibility con vista anterior)
    public string RNC => Cedula;

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
