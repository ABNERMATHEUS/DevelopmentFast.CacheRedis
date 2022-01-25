using DevelopmentFast.Cache.Redis.Repository;
using Microsoft.Extensions.Caching.Distributed;

namespace DevelopmentFast.Cache.UnitTest.Arrange;

public class StudentRepository : BaseRedisRepositoryDF
{
    public StudentRepository(IDistributedCache rd_context) : base(rd_context)
    {
    }
}