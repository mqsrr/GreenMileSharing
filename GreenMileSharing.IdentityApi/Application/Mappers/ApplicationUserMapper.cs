using GreenMileSharing.IdentityApi.Application.Contracts.Requests;
using GreenMileSharing.IdentityApi.Application.Models;
using Riok.Mapperly.Abstractions;

namespace GreenMileSharing.IdentityApi.Application.Mappers;

[Mapper]
internal static partial class ApplicationUserMapper
{
    public static partial ApplicationUser ToUser(this RegisterRequest request);
}