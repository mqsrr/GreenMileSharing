using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace GreenMileSharing.Client.HttpHandlers;

internal sealed class BearerAuthorizationMessageHandler() : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", StaticStorage.Token);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}