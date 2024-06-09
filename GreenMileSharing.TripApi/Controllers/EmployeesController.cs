using GreenMileSharing.TripApi.Application.Helpers;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenMileSharing.TripApi.Controllers;

[ApiController]
[Authorize]
public sealed class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet(ApiEndpoints.Employee.GetById)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
        return employee is not null
            ? Ok(employee)
            : NotFound();
    }

    [HttpGet(ApiEndpoints.Employee.GetByUsername)]
    public async Task<IActionResult> GetByUsername([FromRoute] string username, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByUsernameAsync(username, cancellationToken);
        return employee is not null
            ? Ok(employee)
            : NotFound();
    }

    [HttpPut(ApiEndpoints.Employee.UpdateUsername)]
    public async Task<IActionResult> UpdateUsername([FromRoute] Guid id, [FromQuery] string username, CancellationToken cancellationToken)
    {
        bool isUpdated = await _employeeRepository.UpdateUsernameAsync(id, username, cancellationToken);
        return isUpdated
            ? NoContent()
            : NotFound();
    }

    [HttpDelete(ApiEndpoints.Employee.DeleteById)]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        bool isDeleted = await _employeeRepository.DeleteByIdAsync(id, cancellationToken);
        return isDeleted
            ? NoContent()
            : NotFound();
    }
}