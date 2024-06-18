namespace GreenMileSharing.Messages.Json;

public interface RegisterEmployeeJson
{
    Guid Id { get; }
    
    string Username { get; }
}