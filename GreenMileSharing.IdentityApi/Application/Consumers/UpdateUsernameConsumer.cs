using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.Messages;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace GreenMileSharing.IdentityApi.Application.Consumers;

internal sealed class UpdateUsernameConsumer : IConsumer<UpdateUsername>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUsernameConsumer(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Consume(ConsumeContext<UpdateUsername> context)
    {
        var applicationUser = await _userManager.FindByIdAsync(context.Message.Id.ToString());
        if (applicationUser is null)
        {
            return;
        }

        await _userManager.SetUserNameAsync(applicationUser, context.Message.Username).ConfigureAwait(false);
    }
}