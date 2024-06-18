using Asp.Versioning;
using GreenMileSharing.TripApi.Application.Contracts.Requests.Trips;
using GreenMileSharing.TripApi.Application.Helpers;
using GreenMileSharing.TripApi.Application.Mappers;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenMileSharing.TripApi.Controllers.V2;

[ApiVersion(2.0)]
[ApiController]
[Authorize]
public sealed class TripsController : ControllerBase
{
    private readonly ITripRepository _tripRepository;

    public TripsController(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }
    
    [HttpPost(ApiEndpoints.Trip.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTripRequest request, CancellationToken cancellationToken)
    {
        var createdRequest = await _tripRepository.CreateAsync(request.ToTrip(), cancellationToken);
        return createdRequest is not null
            ? Created(ApiEndpoints.Trip.Create, createdRequest)
            : BadRequest();
    }
    
    [HttpDelete(ApiEndpoints.Trip.DeleteById)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        bool isDeleted = await _tripRepository.DeleteByIdAsync(id, cancellationToken);
        return isDeleted
            ? NoContent()
            : NotFound();
    }
}