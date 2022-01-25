using System;
using System.Threading.Tasks;
using AutoFixture;
using DevelopmentFast.Cache.Redis.Domain.Interfaces.Repository;
using DevelopmentFast.Cache.Redis.Repository;
using DevelopmentFast.Cache.UnitTest.DomainTests;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Moq;
using StackExchange.Redis;
using Xunit;
using Xunit.Abstractions;

namespace DevelopmentFast.Cache.UnitTest;

public class UnitTestRepositoryCache
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Fixture _fixture = new Fixture();

    public UnitTestRepositoryCache(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

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
                    .Returns<string>(_ => student);
            });
        var repositoryCache = repositoryCacheMock.Object;

        //act
        repositoryCache.CreateOrUpdate(key, student, TimeSpan.MaxValue);
        var getValueCache = repositoryCache.Get<StudentTest>(key);

        //assert
        if (getValueCache != null)
        {
            Assert.Equal(student.Id, getValueCache.Id);
        }
        else
        {
            Assert.False(true);
        }
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
            .Callback( () =>
            {
                repositoryCacheMock.Setup(x => x.GetAsync<StudentTest>(It.Is<string>(y => y == key)))
                    .Returns<string>(_ => Task.Run(() => student));
            });
        var repositoryCache = repositoryCacheMock.Object;

        //act
        await repositoryCache.CreateOrUpdateAsync(key, student, TimeSpan.MaxValue);
        var getValueCache = await repositoryCache.GetAsync<StudentTest>(key);


        //assert
        if (getValueCache != null)
        {
            Assert.Equal(student.Id, getValueCache.Id);
        }
        else
        {
            Assert.False(true);
        }
        repositoryCacheMock.Verify(x=> x.GetAsync<StudentTest>(It.Is<string>(y => y == key)),Times.Once);
        repositoryCacheMock.Verify(x=>x.CreateOrUpdateAsync(key, student, TimeSpan.MaxValue),Times.Once);
    }

    [Fact]
    public async void TestConnectionCacheRedis()
    {
        try
        {
            //arrange
            var option = new RedisCacheOptions
            {
                Configuration = "localhost:6379"
            };
            var redisCache = new RedisCache(option);
            
            var student = _fixture.Create<StudentTest>();
            var key = student.Id;
            var repositoryCache = new BaseRedisRepositoryDF(redisCache);

            //act
            await repositoryCache.CreateOrUpdateAsync(key, student, TimeSpan.MaxValue);
            var getValueCache = await repositoryCache.GetAsync<StudentTest>(key);


            //assert
            if (getValueCache != null)
            {

                Assert.Equal(student.Id, getValueCache.Id);
                await redisCache.RefreshAsync(student.Id);
            }
            else
            {
                Assert.False(true);
            }
        }
        catch (RedisConnectionException ex)
        {
            _testOutputHelper.WriteLine(ex.Message);
            throw;
        }

    }
}