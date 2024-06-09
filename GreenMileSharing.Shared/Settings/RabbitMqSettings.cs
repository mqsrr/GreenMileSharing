namespace GreenMileSharing.Shared.Settings;

public sealed class RabbitMqSettings
{
    public const string SectionName = "RabbitMq";
    
    public required string Host { get; init; }
    
    public required string VirtualHost { get; init; }
    
    public required string Username { get; init; }
    
    public required string Password { get; init; }
}