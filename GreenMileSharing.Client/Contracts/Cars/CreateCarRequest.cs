
namespace GreenMileSharing.Client.Contracts.Cars;

internal sealed class CreateCarRequest
{
    public byte[] Image { get; set; } = null!;

    public string LicensePlateNumber { get; set; } = null!;

    public string CarBrand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string EndOfLifeMileage { get; set; } = null!;

    public string MaintenanceInterval { get; set; } = null!;

    public string CurrentMileage { get; set; } = null!;
}