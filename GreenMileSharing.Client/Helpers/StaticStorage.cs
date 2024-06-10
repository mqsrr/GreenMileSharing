using GreenMileSharing.Client.Models;

namespace GreenMileSharing.Client.Helpers;

internal static class StaticStorage
{
    public static Employee Employee { get; internal set; } = null!;
    
    public static string Token { get; internal set; } = null!;
}