using GreenMileSharing.Domain;
using GreenMileSharing.TripApi.Application.Options;

namespace GreenMileSharing.TripApi.Application.Repositories.Abstractions;

public interface ICarRepository
{
    Task<Car?> CreateAsync(Car car, CancellationToken cancellationToken);
    
    Task<IEnumerable<Car>> GetAllAsync(GetAllOptions options, CancellationToken cancellationToken);
    
    Task<Car?> UpdateAsync(Car car, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
}