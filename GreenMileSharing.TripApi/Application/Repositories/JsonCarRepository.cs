using GreenMileSharing.Domain;
using GreenMileSharing.TripApi.Application.Helpers;
using GreenMileSharing.TripApi.Application.Options;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using Newtonsoft.Json;

namespace GreenMileSharing.TripApi.Application.Repositories;

internal sealed class JsonCarRepository : ICarRepository
{
    public async Task<Car?> CreateAsync(Car car, CancellationToken cancellationToken)
    {
        if (!File.Exists(FileStorageNames.Cars))
        {
            File.Create(FileStorageNames.Cars).Close();
        }
        
        string carsJson = await File.ReadAllTextAsync(FileStorageNames.Cars, cancellationToken);
        var cars = JsonConvert.DeserializeObject<List<Car>>(carsJson) ?? [];

        cars.Add(car);
        await File.WriteAllTextAsync(FileStorageNames.Cars, JsonConvert.SerializeObject(cars), cancellationToken);
        
        return car;
    }

    public async Task<IEnumerable<Car>> GetAllAsync(GetAllOptions options, CancellationToken cancellationToken)
    {
        if (!File.Exists(FileStorageNames.Cars))
        {
            File.Create(FileStorageNames.Cars).Close();
        }
        
        string carsJson = await File.ReadAllTextAsync(FileStorageNames.Cars, cancellationToken);
        return JsonConvert.DeserializeObject<List<Car>>(carsJson) ?? [];
    }

    public async Task<Car?> UpdateAsync(Car car, CancellationToken cancellationToken)
    {
        if (!File.Exists(FileStorageNames.Cars))
        {
            File.Create(FileStorageNames.Cars).Close();
        }
        
        string carsJson = await File.ReadAllTextAsync(FileStorageNames.Cars, cancellationToken);
        var cars = JsonConvert.DeserializeObject<List<Car>>(carsJson) ?? [];
        
        int isDeleted = cars.RemoveAll(c => c.Id == car.Id);
        if (isDeleted is 0)
        {
            return null;
        }

        cars.Add(car);
        
        await File.WriteAllTextAsync(FileStorageNames.Cars, JsonConvert.SerializeObject(cars), cancellationToken);
        return car;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (!File.Exists(FileStorageNames.Cars))
        {
            File.Create(FileStorageNames.Cars).Close();
        }
        
        if (!File.Exists(FileStorageNames.Employees))
        {
            File.Create(FileStorageNames.Employees).Close();
        }
        
        string carsJson = await File.ReadAllTextAsync(FileStorageNames.Cars, cancellationToken);
        string employeesJson = await File.ReadAllTextAsync(FileStorageNames.Employees, cancellationToken);
        
        var cars = JsonConvert.DeserializeObject<List<Car>>(carsJson) ?? [];
        var employees = JsonConvert.DeserializeObject<List<Employee>>(employeesJson) ?? [];
        
        int isDeleted = cars.RemoveAll(car => car.Id == id);
        foreach (var employee in employees)
        {
            employee.Trips = employee.Trips?.Where(trip => trip.CarId != id).ToList();
        }
        
        await File.WriteAllTextAsync(FileStorageNames.Cars, JsonConvert.SerializeObject(cars), cancellationToken);
        await File.WriteAllTextAsync(FileStorageNames.Employees, JsonConvert.SerializeObject(employees), cancellationToken);

        return isDeleted > 0;
    }
}