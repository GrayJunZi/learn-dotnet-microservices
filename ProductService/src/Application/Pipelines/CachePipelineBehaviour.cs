using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Application.Pipelines;

public class CachePipelineBehaviour<TRequest, TResponse>(
    IDistributedCache cache,
    IOptions<CacheSettings> cacheSettings) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICacheable
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
            return await next(cancellationToken);


        var cacheKey = $"{cacheSettings.Value.ApplicationName}:{request.CacheKey}";
        var cacheResponse = await cache.GetAsync(cacheKey, cancellationToken);
        if (cacheResponse != null)
            return JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(cacheResponse));

        var response = await next(cancellationToken);

        if (response != null)
        {
            var slidingExpiration = request.SlidingExpiration == null
                ? TimeSpan.FromMinutes(cacheSettings.Value.SlidingExpiration)
                : request.SlidingExpiration;

            var cacheOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration,
                AbsoluteExpiration = DateTime.Now.AddDays(1)
            };

            var serializedData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

            await cache.SetAsync(cacheKey, serializedData, cacheOptions, cancellationToken);
        }

        return response;
    }
}
