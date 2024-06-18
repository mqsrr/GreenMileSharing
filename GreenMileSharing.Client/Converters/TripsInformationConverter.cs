using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using GreenMileSharing.Client.Models;

namespace GreenMileSharing.Client.Converters;

internal sealed class TripsInformationConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var trips = value as ObservableCollection<Trip>;

        return
            $"You registered {trips!.Count} and you have spent {trips.Select(trip => trip.EndMileage - trip.StartMileage).Sum()} miles!";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}