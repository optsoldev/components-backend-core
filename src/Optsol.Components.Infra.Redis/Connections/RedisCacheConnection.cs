using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Settings;
using StackExchange.Redis;
using System;

namespace Optsol.Components.Infra.Redis.Connections
{
    public class RedisCacheConnection: IDisposable
    {
        private bool _disposed = false;

        private readonly ConnectionMultiplexer _connectionMultiplexer;

        private readonly ILogger _logger;

        public RedisCacheConnection(RedisSettings redisSettings, ILoggerFactory logger)
        {
            _logger = logger?.CreateLogger<RedisCacheConnection>();
            _logger?.LogInformation("Inicializando RedisCacheConnection");

            var settingsNotNull = redisSettings == null;
            if (settingsNotNull)
            {
                throw new ArgumentNullException(nameof(redisSettings));
            }

            _connectionMultiplexer = ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
        }

        public IDatabase GetDatabase()
        {
            _logger?.LogInformation($"Método: { nameof(GetDatabase) }() Retorno: IDatabase");

            return _connectionMultiplexer.GetDatabase();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            _logger?.LogInformation($"Método: { nameof(Dispose) }()");

            if (!_disposed && disposing)
            {
                _connectionMultiplexer.Dispose();
            }
            _disposed = true;
        }
    }
}
