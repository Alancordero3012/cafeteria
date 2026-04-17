namespace CafeteriaSystem.Domain.Entities;

public enum SaleStatus { Pendiente, Completada, Cancelada, Devuelta }

// Enum de conveniencia para la UI (se mapea a/desde PaymentMethodEntity.Name)
public enum PaymentMethod { Efectivo, Tarjeta, Transferencia, Mixto }

public class Sale
{
    public int Id { get; set; }                                       // id_venta
    public int? UserId { get; set; }                                  // id_usuario
    public User? User { get; set; }
    public int? CustomerId { get; set; }                              // id_cliente
    public Customer? Customer { get; set; }
    public int? PaymentMethodId { get; set; }                         // id_metodo (FK)
    public PaymentMethodEntity? PaymentMethodEntity { get; set; }
    public string SaleNumber { get; set; } = string.Empty;           // numero_factura
    public DateTime SaleDate { get; set; } = DateTime.Now;           // fecha
    public decimal Subtotal { get; set; }                            // subtotal
    public decimal DiscountAmount { get; set; }                      // descuento
    public decimal TaxAmount { get; set; }                           // itbis
    public decimal Total { get; set; }                               // total
    public decimal AmountPaid { get; set; }                          // monto_pagado
    public decimal Change { get; set; }                              // cambio
    public SaleStatus Status { get; set; } = SaleStatus.Completada; // estado
    public string Notes { get; set; } = string.Empty;               // observaciones

    public ICollection<SaleDetail> Details { get; set; } = new List<SaleDetail>();

    // Propiedad calculada para compatibilidad con la UI
    public PaymentMethod PaymentMethod =>
        PaymentMethodEntity?.Name switch
        {
            "Tarjeta" => PaymentMethod.Tarjeta,
            "Transferencia" => PaymentMethod.Transferencia,
            _ => PaymentMethod.Efectivo
        };

    // CashRegister ya no está en el SQL directamente, se elimina esa relación
    // public int? CashRegisterId { get; set; }
    // public CashRegister? CashRegister { get; set; }
}
