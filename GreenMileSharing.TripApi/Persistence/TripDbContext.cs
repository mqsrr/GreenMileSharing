using System.Reflection;
using GreenMileSharing.Domain;
using Microsoft.EntityFrameworkCore;

namespace GreenMileSharing.TripApi.Persistence;

internal sealed class TripDbContext(DbContextOptions<TripDbContext> options) : DbContext(options)
{
    public required DbSet<Trip> Trips { get; init; }

    public required DbSet<Employee> Employees { get; init; }

    public required DbSet<Car> Cars  { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}