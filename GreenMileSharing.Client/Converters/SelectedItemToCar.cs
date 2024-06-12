using System;
using System.Globalization;
using Avalonia.Data.Converters;
using GreenMileSharing.Client.Models;

namespace GreenMileSharing.Client.Converters;

internal sealed class SelectedItemToCar : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value as Car;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}