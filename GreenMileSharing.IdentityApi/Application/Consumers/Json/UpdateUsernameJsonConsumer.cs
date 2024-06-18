using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.Messages.Json;
using MassTransit;
using Newtonsoft.Json;

namespace GreenMileSharing.IdentityApi.Application.Consumers.Json;

internal sealed class UpdateUsernameJsonConsumer : IConsumer<UpdateUsernameJson>
{
    public async Task Consume(ConsumeContext<UpdateUsernameJson> context)
    {
        if (!File.Exists("identities.json"))
        {
            File.Create("identities.json").Close();
        }
        
        string identitiesJson = await File.ReadAllTextAsync("identities.json", context.CancellationToken);
        var identities = JsonConvert.DeserializeObject<List<ApplicationUser>>(identitiesJson) ?? [];

        var identity = identities.Find(identity => identity.Id == context.Message.Id.ToString());
        if (identity is null)
        {
            return;
        }

        identities.Remove(identity);
        identity.UserName = context.Message.Username;

        identities.Add(identity);
        await File.WriteAllTextAsync("identities.json", JsonConvert.SerializeObject(identities), context.CancellationToken);
    }
}