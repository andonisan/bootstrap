namespace BuildingBlocks.Cache;

public interface ICacheRequest
{
    string CacheKey { get; }
    DateTime? AbsoluteExpirationRelativeToNow { get; }
}
