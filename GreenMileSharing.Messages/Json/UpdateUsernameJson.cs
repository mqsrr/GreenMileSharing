namespace GreenMileSharing.Messages.Json;

public interface UpdateUsernameJson
{
    Guid Id { get; }

    string Username { get; }
}