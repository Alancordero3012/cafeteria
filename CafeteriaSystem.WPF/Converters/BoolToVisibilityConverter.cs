using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CafeteriaSystem.WPF.Converters;

/// <summary>
/// BoolToVisibilityConverter — supports ConverterParameter=Inverse for !value → Visible
/// </summary>
[ValueConversion(typeof(bool), typeof(Visibility))]
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool bVal = value is bool b && b;
        bool inverse = parameter?.ToString()?.Equals("Inverse", StringComparison.OrdinalIgnoreCase) == true;
        if (inverse) bVal = !bVal;
        return bVal ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool inverse = parameter?.ToString()?.Equals("Inverse", StringComparison.OrdinalIgnoreCase) == true;
        bool isVisible = value is Visibility v && v == Visibility.Visible;
        return inverse ? !isVisible : isVisible;
    }
}

[ValueConversion(typeof(decimal), typeof(string))]
public class CurrencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is decimal d ? $"RD${d:N2}" : "RD$0.00";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        decimal.TryParse(value?.ToString()?.Replace("RD$", "").Replace(",", ""), out var d) ? d : 0m;
}
