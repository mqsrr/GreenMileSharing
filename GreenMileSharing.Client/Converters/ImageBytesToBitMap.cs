using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace GreenMileSharing.Client.Converters;

internal sealed class ImageBytesToBitMap : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var bitmap = new Bitmap(new MemoryStream((value as byte[])!));   
        return bitmap;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}