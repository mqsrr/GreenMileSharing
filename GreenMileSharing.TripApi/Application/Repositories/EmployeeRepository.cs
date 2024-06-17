using GreenMileSharing.Domain;
using GreenMileSharing.Messages;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using GreenMileSharing.TripApi.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace GreenMileSharing.TripApi.Application.Repositories;

internal sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly TripDbContext _dbContext;
    private readonly ISendEndpointProvider _endpointProvider;

    public EmployeeRepository(TripDbContext dbContext, ISendEndpointProvider endpointProvider)
    {
        _dbContext = dbContext;
        _endpointProvider = endpointProvider;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken)
    {
        var employees = await _dbContext.Employees
            .Include(employee => employee.Trips)
            .ToListAsync(cancellationToken);

        return employees;
    }

    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var employee = await _dbContext.Employees.FindAsync([id], cancellationToken);
        if (employee is null)
        {
            return null;
        }
        
        await _dbContext.Employees.Entry(employee).Collection( e => e.Trips!).LoadAsync(cancellationToken);
        return employee;
    }

    public async Task<Employee?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var employee = await _dbContext.Employees
            .Include(employee => employee.Trips)
            .FirstOrDefaultAsync(employee => employee.Username == username, cancellationToken);
        
        return employee;
    }

    public async Task<bool> UpdateUsernameAsync(Guid id, string username, CancellationToken cancellationToken)
    {
        int affectedRows = await _dbContext.Employees
            .Where(employee => employee.Id == id)
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(employee => employee.Username, username), cancellationToken);

        bool isUpdated = affectedRows > 0;
        if (isUpdated)
        {
            await _endpointProvider.Send<UpdateUsername>(new
            {
                Id = id,
                Username = username
            }, cancellationToken).ConfigureAwait(false);
        }

        return isUpdated;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        int affectedRows = await _dbContext.Employees
            .Where(employee => employee.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        bool isDeleted = affectedRows > 0;
        if (isDeleted)
        {
            await _endpointProvider.Send<DeleteEmployee>(new
            {
                Id = id
            }, cancellationToken).ConfigureAwait(false);
        }

        return isDeleted;
    }
}