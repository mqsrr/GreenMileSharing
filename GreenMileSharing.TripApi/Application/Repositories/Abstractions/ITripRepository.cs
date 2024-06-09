using GreenMileSharing.Domain;

namespace GreenMileSharing.TripApi.Application.Repositories.Abstractions;

public interface ITripRepository
{
    Task<Trip?> CreateAsync(Trip trip, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
}