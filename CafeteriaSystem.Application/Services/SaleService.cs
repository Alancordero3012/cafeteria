using CafeteriaSystem.Application.DTOs;
using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;

namespace CafeteriaSystem.Application.Services;

public class SaleService(
    ISaleRepository saleRepository,
    IProductRepository productRepository,
    IKardexRepository kardexRepository,
    ITaxRateRepository taxRateRepository)
{
    public async Task<(bool Success, string Message, Sale? Sale)> ProcessSaleAsync(
        List<CartItemDto> cart,
        int userId,
        int? customerId,
        PaymentMethod paymentMethod,
        decimal amountPaid,
        decimal manualDiscountPercent = 0)
    {
        if (cart.Count == 0)
            return (false, "El carrito está vacío.", null);

        // Validate stock
        foreach (var item in cart)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId);
            if (product == null) return (false, $"Producto {item.ProductName} no encontrado.", null);
            if (product.CurrentStock < item.Quantity)
                return (false, $"Stock insuficiente: {item.ProductName} (disponible: {product.CurrentStock})", null);
        }

        var taxRate = await taxRateRepository.GetDefaultAsync();
        var taxPercent = taxRate?.Rate ?? 0.18m;
        var saleNumber = await saleRepository.GetNextSaleNumberAsync();

        decimal subtotal = cart.Sum(i => i.Subtotal);
        decimal itemDiscounts = cart.Sum(i => i.DiscountAmount);
        decimal manualDiscount = subtotal * (manualDiscountPercent / 100);
        decimal totalDiscount = itemDiscounts + manualDiscount;
        decimal taxableBase = subtotal - totalDiscount;
        decimal taxAmount = taxableBase * taxPercent;
        decimal total = taxableBase + taxAmount;
        decimal change = amountPaid - total;

        if (amountPaid < total && paymentMethod == PaymentMethod.Efectivo)
            return (false, $"Monto insuficiente. Falta: {total - amountPaid:N2}", null);

        var sale = new Sale
        {
            SaleNumber = $"V-{saleNumber:D6}",
            SaleDate = DateTime.Now,
            UserId = userId,
            CustomerId = customerId,
            PaymentMethod = paymentMethod,
            Subtotal = subtotal,
            DiscountAmount = totalDiscount,
            TaxAmount = taxAmount,
            Total = total,
            AmountPaid = paymentMethod == PaymentMethod.Efectivo ? amountPaid : total,
            Change = paymentMethod == PaymentMethod.Efectivo ? change : 0,
            Status = SaleStatus.Completada,
            Details = cart.Select(i => new SaleDetail
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                DiscountPercent = i.DiscountPercent,
                TaxPercent = taxPercent * 100,
            }).ToList()
        };

        var savedSale = await saleRepository.AddAsync(sale);

        // Update stock and kardex
        foreach (var item in cart)
        {
            await productRepository.UpdateStockAsync(item.ProductId, -item.Quantity);
            await kardexRepository.AddAsync(new KardexEntry
            {
                ProductId = item.ProductId,
                Date = DateTime.Now,
                Type = KardexType.Salida,
                Quantity = item.Quantity,
                Reason = $"Venta #{savedSale.SaleNumber}",
                ReferenceNumber = savedSale.SaleNumber,
                UserId = userId,
            });
        }

        return (true, "Venta procesada exitosamente.", savedSale);
    }
}
