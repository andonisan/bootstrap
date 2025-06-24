using BuildingBlocks.CQRS.Behaviors;

namespace BuildingBlocks.Cache;

public partial class CachingBehavior<TRequest, TResponse>(
    ICacheService redisService,
    ILogger<CachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheRequest
    where TResponse : class
{
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger = logger;
    private const int DefaultCacheExpirationInHours = 1;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheRequest cacheRequest)
        {
            return await next(cancellationToken);
        }

        var cacheKey = cacheRequest.CacheKey;


        var cachedResponse = await redisService.GetAsync<TResponse>(cacheKey);

        if (cachedResponse != null)
        {
            LogCacheHit(cacheKey);
            return cachedResponse;
        }

        var response = await next(cancellationToken);

        var expirationTime = cacheRequest.AbsoluteExpirationRelativeToNow ??
                             DateTime.Now.AddHours(DefaultCacheExpirationInHours);


        await redisService.SaveAsync(cacheKey, response, expirationTime.TimeOfDay);
        LogCacheSave(cacheKey, expirationTime.TimeOfDay);

        return response;
    }


    [LoggerMessage(
        EventName = nameof(LogCacheHit),
        Level = LogLevel.Information,
        Message = "(CACHE) -- FETCH with cacheKey: {cacheKey}")]
    public partial void LogCacheHit(string cacheKey);

    [LoggerMessage(
        EventName = nameof(LogCacheSave),
        Level = LogLevel.Information,
        Message = "(CACHE) -- SAVE with cacheKey: {cacheKey} and expiration: {expirationTimeTimeOfDay}")]
    public partial void LogCacheSave(string cacheKey, TimeSpan expirationTimeTimeOfDay);
}
