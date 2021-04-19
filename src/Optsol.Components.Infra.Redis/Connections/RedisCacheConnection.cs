using Optsol.Components.Shared.Settings;
using StackExchange.Redis;
using System;

namespace Optsol.Components.Infra.Redis.Connections
{
    public class RedisCacheConnection
    {
        private readonly RedisSettings _redisSettings;
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheConnection(RedisSettings redisSettings)
        {
            _redisSettings = redisSettings ?? throw new ArgumentNullException(nameof(redisSettings));

            _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisSettings.ConnectionString);
        }

        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase();
        }
    }
}
