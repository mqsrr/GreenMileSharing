using GreenMileSharing.Domain;
using GreenMileSharing.TripApi.Application.Contracts.Requests.Cars;
using Riok.Mapperly.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;

namespace GreenMileSharing.TripApi.Application.Mappers;

[Mapper]
internal static partial class CarMapper
{
    internal static partial Car ToCar(this CreateCarRequest request);

    private static partial Car ToCar(this UpdateCarRequest request);

    internal static Car ToCar(this UpdateCarRequest request, Guid id)
    {
        request.Id = id;
        return request.ToCar();
    }
    
    private static byte[] MapImageToByte(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        using var image = Image.Load(file.OpenReadStream());

        image.Save(memoryStream, image.DetectEncoder(file.FileName));
        return memoryStream.ToArray();
    }
}