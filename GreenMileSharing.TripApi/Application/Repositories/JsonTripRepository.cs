using GreenMileSharing.Domain;
using GreenMileSharing.TripApi.Application.Helpers;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using Newtonsoft.Json;

namespace GreenMileSharing.TripApi.Application.Repositories;

internal sealed class JsonTripRepository : ITripRepository
{

    public async Task<Trip?> CreateAsync(Trip trip, CancellationToken cancellationToken)
    {
        if (!File.Exists(FileStorageNames.Employees))
        {
            File.Create(FileStorageNames.Employees).Close();
        } 
        
        if (!File.Exists(FileStorageNames.Cars))
        {
            File.Create(FileStorageNames.Cars).Close();
        }
        
        string employeesJson = await File.ReadAllTextAsync(FileStorageNames.Employees, cancellationToken);
        string carsJson = await File.ReadAllTextAsync(FileStorageNames.Cars, cancellationToken);
        
        var cars = JsonConvert.DeserializeObject<List<Car>>(carsJson) ?? [];
        var employees = JsonConvert.DeserializeObject<List<Employee>>(employeesJson) ?? [];
        
        var carToUpdate = cars.Find(car => car.Id == trip.CarId);
        var employee = employees.Find(employee => employee.Id == trip.EmployeeId);
        
        if (carToUpdate is null || employee is null)
        {
            return null;
        }

        employees.Remove(employee);
        employee.Trips = employee.Trips?.Append(trip) ?? [trip];
        employees.Add(employee);
        
        cars.Remove(carToUpdate);
        carToUpdate.CurrentMileage = trip.EndMileage;
        carToUpdate.Trips = carToUpdate.Trips?.Append(trip) ?? [trip];
        cars.Add(carToUpdate);
        
        await File.WriteAllTextAsync(FileStorageNames.Cars, JsonConvert.SerializeObject(cars), cancellationToken);
        await File.WriteAllTextAsync(FileStorageNames.Employees, JsonConvert.SerializeObject(employees), cancellationToken);

        return trip;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}