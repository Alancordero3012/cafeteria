using CafeteriaSystem.Application.DTOs;
using CafeteriaSystem.Domain.Interfaces;

namespace CafeteriaSystem.Application.Services;

public class ReportService(ISaleRepository saleRepository, IProductRepository productRepository, ICategoryRepository categoryRepository)
{
    public async Task<DashboardDto> GetDashboardAsync()
    {
        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var todaySales = (await saleRepository.GetTodayAsync()).ToList();
        var monthSales = (await saleRepository.GetByDateRangeAsync(monthStart, today.AddDays(1))).ToList();
        var products = (await productRepository.GetActivesAsync()).ToList();
        var categories = (await categoryRepository.GetAllAsync()).ToList();

        // Ventas últimos 7 días
        var ventasPorDia = new List<VentaDiariaDto>();
        for (int i = 6; i >= 0; i--)
        {
            var dia = today.AddDays(-i);
            var daySales = (await saleRepository.GetByDateRangeAsync(dia, dia.AddDays(1))).ToList();
            ventasPorDia.Add(new VentaDiariaDto(
                dia.ToString("ddd"),
                daySales.Sum(s => s.Total),
                daySales.Count));
        }

        // Ventas por categoría (simulado desde los nombres de categoría)
        var categoryColors = new[] { "#E8A87C", "#7C5CBF", "#4FC3F7", "#4CAF50", "#F06292" };
        var weights = new[] { 0.35m, 0.25m, 0.20m, 0.12m, 0.08m };
        var totalMes = monthSales.Sum(s => s.Total);
        var ventasPorCategoria = categories.Select((c, i) => new VentaCategoriaDto(
            c.Name,
            totalMes * weights[Math.Min(i, weights.Length - 1)],
            categoryColors[Math.Min(i, categoryColors.Length - 1)])).ToList();

        // Top productos
        var topProductos = monthSales
            .SelectMany(s => s.Details)
            .GroupBy(d => d.ProductName)
            .Select(g => new ProductoTopDto(g.Key, g.Sum(d => d.Quantity), g.Sum(d => d.Total)))
            .OrderByDescending(p => p.Total)
            .Take(5)
            .ToList();

        return new DashboardDto
        {
            TotalVentasHoy = todaySales.Sum(s => s.Total),
            TotalVentasMes = monthSales.Sum(s => s.Total),
            TotalOrdenesHoy = todaySales.Count,
            TotalProductos = products.Count,
            ProductosStockBajo = products.Count(p => p.HasLowStock),
            TotalCaja = todaySales.Sum(s => s.Total),
            VentasPorDia = ventasPorDia,
            VentasPorCategoria = ventasPorCategoria,
            TopProductos = topProductos,
        };
    }
}
