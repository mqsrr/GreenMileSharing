using GreenMileSharing.Domain;
using GreenMileSharing.Messages.Json;
using GreenMileSharing.TripApi.Application.Mappers;
using MassTransit;
using Newtonsoft.Json;

namespace GreenMileSharing.TripApi.Application.Consumers;

internal sealed class RegisterEmployeeJsonConsumer : IConsumer<RegisterEmployeeJson>
{
    public async Task Consume(ConsumeContext<RegisterEmployeeJson> context)
    {
        if (!File.Exists("employees.json"))
        {
            File.Create("employees.json").Close();
        }
        
        string employeesJson = await File.ReadAllTextAsync("employees.json", context.CancellationToken);
        var employees = JsonConvert.DeserializeObject<List<Employee>>(employeesJson) ?? [];
        
        employees.Add(context.Message.ToEmployee());

        await File.WriteAllTextAsync("employees.json", JsonConvert.SerializeObject(employees), context.CancellationToken);
    }
}