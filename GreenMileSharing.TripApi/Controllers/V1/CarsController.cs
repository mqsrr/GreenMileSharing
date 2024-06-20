using Asp.Versioning;
using GreenMileSharing.TripApi.Application.Contracts.Requests.Cars;
using GreenMileSharing.TripApi.Application.Helpers;
using GreenMileSharing.TripApi.Application.Mappers;
using GreenMileSharing.TripApi.Application.Options;
using GreenMileSharing.TripApi.Application.Repositories;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenMileSharing.TripApi.Controllers.V1;

[ApiVersion(1.0)]
[ApiController]
[Authorize]
public sealed class CarsController : ControllerBase
{
    private readonly ICarRepository _carRepository;

    public CarsController([FromKeyedServices(nameof(JsonCarRepository))]ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    [HttpGet(ApiEndpoints.Car.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery]GetAllOptions options, CancellationToken cancellationToken)
    {
        var cars = await _carRepository.GetAllAsync(options, cancellationToken);
        return Ok(cars);
    }
    
    [HttpPost(ApiEndpoints.Car.Create)]
    public async Task<IActionResult> Create([FromForm] CreateCarRequest request, CancellationToken cancellationToken)
    {
        var createdCar = await _carRepository.CreateAsync(request.ToCar(), cancellationToken);
        return createdCar is not null
            ? Created(ApiEndpoints.Car.Create, createdCar)
            : BadRequest();
    }
    
    [HttpPut(ApiEndpoints.Car.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateCarRequest request, CancellationToken cancellationToken)
    {
        var updatedCart = await _carRepository.UpdateAsync(request.ToCar(id), cancellationToken);
        return updatedCart is not null
            ? Ok(updatedCart)
            : BadRequest();
    }
    
    [HttpDelete(ApiEndpoints.Car.DeleteById)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        bool idDeleted = await _carRepository.DeleteByIdAsync(id, cancellationToken);
        return idDeleted
            ? NoContent()
            : NotFound();
    }
}