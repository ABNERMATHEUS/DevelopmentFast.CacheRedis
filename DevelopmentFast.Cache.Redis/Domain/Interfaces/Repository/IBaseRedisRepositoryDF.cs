using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentFast.Cache.Redis.Domain.Interfaces.Repository
{
    public interface IBaseRedisRepositoryDF 
    {
        T? Get<T>(string key);
        Task<T?> GetAsync<T>(string key);

        void CreateOrUpdate<T>(string key, T obj, TimeSpan timeSpan);
        Task CreateOrUpdateAsync<T>(string key, T obj, TimeSpan timeSpan);
    }
}
