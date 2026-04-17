namespace CafeteriaSystem.Domain.Entities;

public enum KardexType { Entrada, Salida, Ajuste, DevolucionVenta, DevolucionCompra }

public class KardexEntry
{
    public int Id { get; set; }                                      // id_movimiento
    public int? ProductId { get; set; }                              // id_producto
    public Product? Product { get; set; }
    public int? UserId { get; set; }                                 // id_usuario
    public string MovementType { get; set; } = string.Empty;         // tipo_movimiento (string en BD)
    public decimal Quantity { get; set; }                            // cantidad
    public decimal StockBefore { get; set; }                         // stock_anterior
    public decimal StockAfter { get; set; }                          // stock_nuevo
    public DateTime Date { get; set; } = DateTime.Now;              // fecha

    // Compatibilidad con la UI: mapeo entre string y enum
    public KardexType Type
    {
        get => MovementType switch
        {
            "Entrada" => KardexType.Entrada,
            "Salida" => KardexType.Salida,
            "Ajuste" => KardexType.Ajuste,
            "DevolucionVenta" => KardexType.DevolucionVenta,
            "DevolucionCompra" => KardexType.DevolucionCompra,
            _ => KardexType.Ajuste
        };
        set => MovementType = value.ToString();
    }

    // Campos que no existen en el SQL pero se usan en la UI (no mapeados):
    public string Reason { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
}
