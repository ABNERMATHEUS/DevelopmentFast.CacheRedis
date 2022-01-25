using System;
using System.Threading.Tasks;
using AutoFixture;
using DevelopmentFast.Cache.Redis.Domain.Interfaces.Repository;
using DevelopmentFast.Cache.Redis.Extension;
using DevelopmentFast.Cache.Redis.Repository;
using DevelopmentFast.Cache.UnitTest.DomainTests;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StackExchange.Redis;
using Xunit;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace DevelopmentFast.Cache.UnitTest;

public class UnitTestRepositoryCache
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public void CreateOrUpdate_Get()
    {
        //arrange
        var student = _fixture.Create<StudentTest>();
        var key = student.Id;
        var repositoryCacheMock = new Mock<IBaseRedisRepositoryDF>();
        repositoryCacheMock.Setup(x => x.CreateOrUpdate(key, student, TimeSpan.MaxValue))
            .Callback(() =>
            {
                repositoryCacheMock.Setup(x => x.Get<StudentTest>(It.Is<string>(y => y == key)))
                    .Returns<string>(x => student);
            });
        var repositoryCache = repositoryCacheMock.Object;

        //act
        repositoryCache.CreateOrUpdate(key, student, TimeSpan.MaxValue);
        var getValueCache = repositoryCache.Get<StudentTest>(key);

        //assert
        Assert.Equal(student.Id, getValueCache.Id);
        repositoryCacheMock.Verify(x=> x.Get<StudentTest>(It.Is<string>(y => y == key)),Times.Once);
        repositoryCacheMock.Verify(x=>x.CreateOrUpdate(key, student, TimeSpan.MaxValue),Times.Once);
    }

    [Fact]
    public async void CreateOrUpdateAsync_GetAsync()
    {
        //arrange
        var student = _fixture.Create<StudentTest>();
        var key = student.Id;
        var repositoryCacheMock = new Mock<IBaseRedisRepositoryDF>();
        repositoryCacheMock.Setup(x => x.CreateOrUpdateAsync(key, student, TimeSpan.MaxValue))
            .Callback(async () =>
            {
                repositoryCacheMock.Setup(x => x.GetAsync<StudentTest>(It.Is<string>(y => y == key)))
                    .Returns<string>(x => Task.Run(() => student));
            });
        var repositoryCache = repositoryCacheMock.Object;

        //act
        await repositoryCache.CreateOrUpdateAsync(key, student, TimeSpan.MaxValue);
        var getValueCache = await repositoryCache.GetAsync<StudentTest>(key);


        //assert
        Assert.Equal(student.Id, getValueCache.Id);
        repositoryCacheMock.Verify(x=> x.GetAsync<StudentTest>(It.Is<string>(y => y == key)),Times.Once);
        repositoryCacheMock.Verify(x=>x.CreateOrUpdateAsync(key, student, TimeSpan.MaxValue),Times.Once);
    }

    [Fact]
    public async void TestConnectionCacheRedis()
    {
        try
        {

            //IServiceCollection serviceCollection = new ServiceCollection();
            //serviceCollection.AddSingletonRedisCache(x => x.Configuration = "localhost:6379");
            var option = new RedisCacheOptions();
            option.Configuration = "localhost:6379";
            var redisCache = new RedisCache(option);
            //arrange
            var student = _fixture.Create<StudentTest>();
            var key = student.Id;
            var repositoryCache = new BaseRedisRepositoryDF(redisCache);

            //act
            await repositoryCache.CreateOrUpdateAsync(key, student, TimeSpan.MaxValue);
            var getValueCache = await repositoryCache.GetAsync<StudentTest>(key);


            //assert
            Assert.Equal(student.Id, getValueCache.Id);
            await redisCache.RefreshAsync(student.Id);
        }
        catch (RedisConnectionException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}