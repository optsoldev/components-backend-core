using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Redis.Services
{
    public interface IRedisCacheService
    {
        public Task<T> ReadAsync<T>(string key) where T : class;

        public Task SaveAsync<T>(KeyValuePair<string, T> data) where T : class;
    }
}
