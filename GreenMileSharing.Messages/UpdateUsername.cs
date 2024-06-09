namespace GreenMileSharing.Messages;

public interface UpdateUsername
{
    Guid Id { get; }

    string Username { get; }
}