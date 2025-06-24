namespace BuildingBlocks.Cache;

public interface IInvalidateCacheRequest
{
    string PrefixCacheKey { get; }
}
