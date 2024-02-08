namespace Core.Interfaces;

public interface IResponseCacheService
{
    Task CacheResponseASync(string cacheKey, object response, TimeSpan timeToLive);
    Task<string?> GetCachedResponseAsync(string cacheKey);
}
