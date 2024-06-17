using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using Refit;

namespace GreenMileSharing.Client.RefitClients;

[Headers("Bearer")]
internal interface ICarsWebApi
{
    [Get(ApiEndpoints.Car.GetAll)]
    Task<IApiResponse<IEnumerable<Car>>> GetAllAsync(CancellationToken cancellationToken);
    
    [Multipart]
    [Post(ApiEndpoints.Car.GetAll)]
    Task<IApiResponse<Car>> CreateAsync(
        [AliasAs("Image")] StreamPart image,
        [AliasAs("LicensePlateNumber")] string licensePlateNumber,
        [AliasAs("CarBrand")] string carBrand,
        [AliasAs("Model")] string model,
        [AliasAs("EndOfLifeMileage")] int endOfLifeMileage,
        [AliasAs("MaintenanceInterval")] int maintenanceInterval,
        [AliasAs("CurrentMileage")] int currentMileage);
    
    [Delete(ApiEndpoints.Car.DeleteById)]
    Task<IApiResponse> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

}