using CafeteriaSystem.Application.DTOs;
using CafeteriaSystem.Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView;

namespace CafeteriaSystem.WPF.ViewModels;

public partial class DashboardViewModel(ReportService reportService) : BaseViewModel
{
    [ObservableProperty] private DashboardDto? _dashboard;
    [ObservableProperty] private bool _hasData;

    // LiveCharts2 series — assigned after load
    [ObservableProperty] private object? _barSeriesCollection;
    [ObservableProperty] private object? _pieSeriesCollection;
    [ObservableProperty] private IEnumerable<Axis>? _barXAxes;
    [ObservableProperty] private IEnumerable<Axis>? _barYAxes;

    public override async Task OnNavigatedToAsync()
    {
        IsBusy = true;
        await Task.Delay(300);
        Dashboard = await reportService.GetDashboardAsync();
        BuildCharts();
        HasData = Dashboard != null;
        IsBusy = false;
    }

    private void BuildCharts()
    {
        if (Dashboard == null) return;

        // Bar chart: ventas por día
        var values = Dashboard.VentasPorDia.Select(v => v.Total).ToArray();
        var labels = Dashboard.VentasPorDia.Select(v => v.Dia).ToArray();
        BarXAxes = new Axis[] { new() { Labels = labels, LabelsPaint = null, SeparatorsPaint = null, TicksPaint = null } };
        BarYAxes = new Axis[] { new() { LabelsPaint = null, SeparatorsPaint = null, TicksPaint = null } };
        BarSeriesCollection = new LiveChartsCore.SkiaSharpView.ColumnSeries<decimal>[]
        {
            new()
            {
                Values = values,
                Fill = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SkiaSharp.SKColor.Parse("#E8A87C")),
                Stroke = null,
                MaxBarWidth = 40,
                Name = "Ventas RD$"
            }
        };

        // Pie chart: por categoría
        PieSeriesCollection = Dashboard.VentasPorCategoria.Select(c =>
            new LiveChartsCore.SkiaSharpView.PieSeries<decimal>
            {
                Values = new[] { c.Total },
                Name = c.Categoria,
                Fill = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SkiaSharp.SKColor.Parse(c.Color)),
                OuterRadiusOffset = 0,
            }).ToArray();
    }
}
