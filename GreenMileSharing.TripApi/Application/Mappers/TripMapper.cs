using GreenMileSharing.Domain;
using GreenMileSharing.TripApi.Application.Contracts.Requests.Trips;
using Riok.Mapperly.Abstractions;

namespace GreenMileSharing.TripApi.Application.Mappers;

[Mapper]
internal static partial class TripMapper
{
    internal static partial Trip ToTrip(this CreateTripRequests request);
}