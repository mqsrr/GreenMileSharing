using GreenMileSharing.Domain;
using GreenMileSharing.TripApi.Application.Options;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using GreenMileSharing.TripApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GreenMileSharing.TripApi.Application.Repositories;

internal sealed class CarRepository : ICarRepository
{
    private readonly TripDbContext _dbContext;

    public CarRepository(TripDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Car?> CreateAsync(Car car, CancellationToken cancellationToken)
    {
        await _dbContext.Cars.AddAsync(car, cancellationToken);
        int affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);

        return affectedRows > 0
            ? car
            : null;
    }
    
    public async Task<IEnumerable<Car>> GetAllAsync(GetAllOptions options, CancellationToken cancellationToken)
    {
        var cars = await  _dbContext.Cars
            .Include(car => car.Trips)
            .ToListAsync(cancellationToken);
        
        return cars;
    }

    public async Task<Car?> UpdateAsync(Car car, CancellationToken cancellationToken)
    {
        _dbContext.Cars.Update(car);
        int affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);

        return affectedRows > 0
            ? car
            : null;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        int affectedRows = await _dbContext.Cars
            .Where(car => car.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return affectedRows > 0;
    }
}