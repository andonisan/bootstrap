using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Cache;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddCaching(this WebApplicationBuilder builder)
    {
        builder.AddRedisClient("cache");

        builder.Services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(InvalidateCachingBehavior<,>))
            .AddScoped<ICacheService, RedisCacheService>();

        return builder;
    }
}
