using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace DevelopmentFast.Cache.Redis.Extension;

public static class ServicesExtension
{
    public static void AddSingletonRedisCacheDF(this IServiceCollection service,Action<RedisCacheOptions> options)
    {
        service.AddSingleton<IDistributedCache, RedisCache>();
        service.AddStackExchangeRedisCache(options);
    }
    
    public static void AddTransientRedisCacheDF(this IServiceCollection service,Action<RedisCacheOptions> options)
    {
        service.AddTransient<IDistributedCache, RedisCache>();
        service.AddStackExchangeRedisCache(options);
    }
    
    public static void AddScopedRedisCacheDF(this IServiceCollection service, Action<RedisCacheOptions> options)
    {
        service.AddScoped<IDistributedCache, RedisCache>();
        service.AddStackExchangeRedisCache(options);
    }
}