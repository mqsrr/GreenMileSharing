using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.Messages;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace GreenMileSharing.IdentityApi.Application.Consumers;

internal sealed class DeleteEmployeeConsumer : IConsumer<DeleteEmployee>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteEmployeeConsumer(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Consume(ConsumeContext<DeleteEmployee> context)
    {
        var applicationUser = await _userManager.FindByIdAsync(context.Message.Id.ToString());
        if (applicationUser is null)
        {
            return;
        }

        await _userManager.DeleteAsync(applicationUser).ConfigureAwait(false);
    }
}