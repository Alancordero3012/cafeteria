namespace CafeteriaSystem.Application.DTOs;

public record DashboardDto
{
    public decimal TotalVentasHoy { get; init; }
    public decimal TotalVentasMes { get; init; }
    public int TotalOrdenesHoy { get; init; }
    public int TotalProductos { get; init; }
    public int ProductosStockBajo { get; init; }
    public int TotalClientes { get; init; }
    public decimal TotalCaja { get; init; }
    public IEnumerable<VentaDiariaDto> VentasPorDia { get; init; } = [];
    public IEnumerable<VentaCategoriaDto> VentasPorCategoria { get; init; } = [];
    public IEnumerable<ProductoTopDto> TopProductos { get; init; } = [];
}

public record VentaDiariaDto(string Dia, decimal Total, int Ordenes);
public record VentaCategoriaDto(string Categoria, decimal Total, string Color);
public record ProductoTopDto(string Nombre, int Cantidad, decimal Total);
