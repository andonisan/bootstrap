using BuildingBlocks.Domain;

namespace BuildingBlocks.Cache;

public partial class InvalidateCachingBehavior<TRequest, TResponse>(
    ILogger<InvalidateCachingBehavior<TRequest, TResponse>> logger,
    ICacheService redisCacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IInvalidateCacheRequest
    where TResponse : notnull
{
    private readonly ILogger<InvalidateCachingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {

        if (request is not IInvalidateCacheRequest cacheRequest)
        {
            return await next(cancellationToken);
        }

        var response = await next(cancellationToken);

        if (response is not Result { IsSuccess: true })
        {
            return response;
        }

        var cacheKey = cacheRequest.PrefixCacheKey;

        await redisCacheService.DeleteAsync(cacheKey);

        LogInvalidateCache(cacheKey);

        return response;
    }

    [LoggerMessage(
        EventName = nameof(LogInvalidateCache),
        Level = LogLevel.Information,
        Message = "(CACHE) -- INVALIDATE with cacheKey: {cacheKey} removed.")]
    public partial void LogInvalidateCache(string cacheKey);
}
