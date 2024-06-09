using GreenMileSharing.Domain;

namespace GreenMileSharing.TripApi.Application.Repositories.Abstractions;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Employee?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    
    Task<bool> UpdateUsernameAsync(Guid id, string username, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
}