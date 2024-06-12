using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace GreenMileSharing.Client.Converters;

internal sealed class StringToIntConverter : IValueConverter
{

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value!.ToString()!;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(value?.ToString()))
        {
            return null;
        }
        
        return System.Convert.ToInt32(value.ToString());
    }
}