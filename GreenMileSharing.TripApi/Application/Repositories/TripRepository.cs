using GreenMileSharing.Domain;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using GreenMileSharing.TripApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GreenMileSharing.TripApi.Application.Repositories;

internal sealed class TripRepository : ITripRepository
{
    private readonly TripDbContext _dbContext;

    public TripRepository(TripDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Trip?> CreateAsync(Trip trip, CancellationToken cancellationToken)
    {
        await _dbContext.Trips.AddAsync(trip, cancellationToken);
        int affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
        
        bool isCreated = affectedRows > 0;
        if (!isCreated)
        {
            return null;
        }

        await _dbContext.Cars
            .Where(car => car.Id == trip.CarId)
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(car => car.CurrentMileage, trip.StartMileage + trip.EndMileage), cancellationToken);
        
        return trip;
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        int affectedRows = await _dbContext.Trips
            .Where(trip => trip.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return affectedRows > 0;
    }
}