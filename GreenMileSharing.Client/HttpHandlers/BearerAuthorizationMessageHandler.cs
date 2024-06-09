using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace GreenMileSharing.Client.HttpHandlers;

internal sealed class BearerAuthorizationMessageHandler(IMemoryCache memoryCache) : DelegatingHandler
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _memoryCache.Get<string>(CacheKeys.JwtToken));
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}