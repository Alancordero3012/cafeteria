namespace CafeteriaSystem.Domain.Entities;

/// <summary>
/// Mapea la tabla Configuracion_Empresa del esquema sistema_ventasbd.
/// Almacena pares clave-valor de configuración de la empresa.
/// </summary>
public class CompanyConfig
{
    public int Id { get; set; }                      // id_config
    public string Key { get; set; } = string.Empty;  // clave
    public string Value { get; set; } = string.Empty; // valor
}
