using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace DevelopmentFast.Cache.Redis.Extension;

public static class ServicesExtension
{
    public static void AddSingletonRedisCache(this IServiceCollection service,Action<RedisCacheOptions> options)
    {
        service.AddSingleton<IDistributedCache, RedisCache>();
        service.AddStackExchangeRedisCache(options);
    }
    
    public static void AddTransientRedisCache(this IServiceCollection service,Action<RedisCacheOptions> options)
    {
        service.AddTransient<IDistributedCache, RedisCache>();
        service.AddStackExchangeRedisCache(options);
    }
    
    public static void AddScopedRedisCache(this IServiceCollection service, Action<RedisCacheOptions> options)
    {
        service.AddScoped<IDistributedCache, RedisCache>();
        service.AddStackExchangeRedisCache(options);
    }
}