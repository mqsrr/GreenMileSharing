using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.Messages.Json;
using MassTransit;
using Newtonsoft.Json;

namespace GreenMileSharing.IdentityApi.Application.Consumers.Json;

internal sealed class DeleteEmployeeJsonConsumer : IConsumer<DeleteEmployeeJson>
{
    public async Task Consume(ConsumeContext<DeleteEmployeeJson> context)
    {
        if (!File.Exists("identities.json"))
        {
            File.Create("identities.json").Close();
        }
        
        string identitiesJson = await File.ReadAllTextAsync("identities.json", context.CancellationToken);
        var identities = JsonConvert.DeserializeObject<List<ApplicationUser>>(identitiesJson) ?? [];

        identities.RemoveAll(identity => identity.Id == context.Message.Id.ToString());
        await File.WriteAllTextAsync("identities.json", JsonConvert.SerializeObject(identities), context.CancellationToken);
    }
}