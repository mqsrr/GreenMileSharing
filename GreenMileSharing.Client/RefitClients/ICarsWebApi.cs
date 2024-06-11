using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Contracts.Cars;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using Refit;

namespace GreenMileSharing.Client.RefitClients;

[Headers("Bearer")]
internal interface ICarsWebApi
{
    [Get(ApiEndpoints.Car.GetAll)]
    Task<IApiResponse<IEnumerable<Car>>> GetAllAsync(CancellationToken cancellationToken);
    
    [Post(ApiEndpoints.Car.Create)]
    Task<IApiResponse<Car>> CreateAsync([Body(BodySerializationMethod.UrlEncoded)] CreateCarRequest request, CancellationToken cancellationToken);
}