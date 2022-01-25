using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace DevelopmentFast.Cache.Redis.Extension;

public static class ServicesExtension
{
    public static IServiceCollection AddSingletonRedisCacheDF(this IServiceCollection service,Action<RedisCacheOptions> options)
    {
        service.AddSingleton<IDistributedCache, RedisCache>();
        service.AddStackExchangeRedisCache(options);
        return service;
    }
    
    public static IServiceCollection AddTransientRedisCacheDF(this IServiceCollection service,Action<RedisCacheOptions> options)
    {
        service.AddTransient<IDistributedCache, RedisCache>();
        service.AddStackExchangeRedisCache(options);
        return service;
    }
    
    public static IServiceCollection AddScopedRedisCacheDF(this IServiceCollection service, Action<RedisCacheOptions> options)
    {
        service.AddScoped<IDistributedCache, RedisCache>();
        service.AddStackExchangeRedisCache(options);
        return service;
    }
}