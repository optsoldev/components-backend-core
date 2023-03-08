using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Redis.Services;

public interface IRedisCacheService
{
    Task<T> ReadAsync<T>(string key) where T : class;

    Task SaveAsync<T>(KeyValuePair<string, T> data) where T : class;

    Task SaveAsync<T>(KeyValuePair<string, T> data, int expirationInMinutes) where T : class;

    Task DeleteAsync(string key);
}