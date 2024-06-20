using GreenMileSharing.Domain;
using GreenMileSharing.Messages.Json;
using GreenMileSharing.TripApi.Application.Helpers;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using MassTransit;
using Newtonsoft.Json;

namespace GreenMileSharing.TripApi.Application.Repositories;

internal sealed class JsonEmployeeRepository : IEmployeeRepository
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    
    public JsonEmployeeRepository(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(FileStorageNames.Employees))
        {
            File.Create(FileStorageNames.Employees).Close();
        }
        
        string employeesJson = await File.ReadAllTextAsync(FileStorageNames.Employees, cancellationToken);
        return JsonConvert.DeserializeObject<List<Employee>>(employeesJson) ?? [];
    }

    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (!File.Exists(FileStorageNames.Employees))
        {
            File.Create(FileStorageNames.Employees).Close();
        }

        string employeesJson = await File.ReadAllTextAsync(FileStorageNames.Employees, cancellationToken);
        var employees = JsonConvert.DeserializeObject<List<Employee>>(employeesJson) ?? [];

        return employees.Find(employee => employee.Id == id);
    }

    public async Task<Employee?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        if (!File.Exists(FileStorageNames.Employees))
        {
            File.Create(FileStorageNames.Employees).Close();
        }

        string employeesJson = await File.ReadAllTextAsync(FileStorageNames.Employees, cancellationToken);
        var employees = JsonConvert.DeserializeObject<List<Employee>>(employeesJson) ?? [];

        return employees.Find(employee => employee.Username == username);
    }

    public Task<bool> UpdateUsernameAsync(Guid id, string username, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
        
        int isDeleted = employees.RemoveAll(employee => employee.Id == id);
        foreach (var car in cars)
        {
            car.Trips = car.Trips?.Where(trip => trip.EmployeeId != id).ToList();
        }

        await _sendEndpointProvider.Send<DeleteEmployeeJson>(new
        {
            Id = id
        }, cancellationToken);
        
        await File.WriteAllTextAsync(FileStorageNames.Cars, JsonConvert.SerializeObject(cars), cancellationToken);
        await File.WriteAllTextAsync(FileStorageNames.Employees, JsonConvert.SerializeObject(employees), cancellationToken);
        
        return isDeleted > 0;
    }
}