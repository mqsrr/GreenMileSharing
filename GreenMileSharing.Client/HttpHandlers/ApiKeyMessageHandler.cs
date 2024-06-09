using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GreenMileSharing.Client.HttpHandlers;

internal sealed class ApiKeyMessageHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}