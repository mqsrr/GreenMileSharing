namespace GreenMileSharing.Messages;

public interface RegisterEmployee
{
    Guid Id { get; }
    
    string Username { get; }
}