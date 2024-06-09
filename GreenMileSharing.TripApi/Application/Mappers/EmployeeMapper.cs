using GreenMileSharing.Domain;
using GreenMileSharing.Messages;
using Riok.Mapperly.Abstractions;

namespace GreenMileSharing.TripApi.Application.Mappers;

[Mapper]
internal static partial class EmployeeMapper
{
    internal static partial Employee ToEmployee(this RegisterEmployee registerEmployee);
}