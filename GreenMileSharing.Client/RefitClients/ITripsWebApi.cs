using System;
using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Contracts.Trips;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using Refit;

namespace GreenMileSharing.Client.RefitClients;

[Headers("Bearer")]
internal interface ITripsWebApi
{
    [Post(ApiEndpoints.Trip.Create)]
    Task<IApiResponse<Trip>> CreateAsync(string apiVersion, [Body] CreateTripRequest request, CancellationToken cancellationToken);

    [Delete(ApiEndpoints.Trip.DeleteById)]
    Task<IApiResponse> DeleteByIdAsync(string apiVersion, Guid id);
}