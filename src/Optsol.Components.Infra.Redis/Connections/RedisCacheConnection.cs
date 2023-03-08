using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Settings;
using StackExchange.Redis;
using System;

namespace Optsol.Components.Infra.Redis.Connections;

public class RedisCacheConnection
{
    private readonly Lazy<ConnectionMultiplexer> connectionMultiplexer;

    private readonly ILogger logger;

    public RedisCacheConnection(RedisSettings redisSettings, ILoggerFactory logger)
    {
        this.logger = logger?.CreateLogger<RedisCacheConnection>();
        this.logger?.LogInformation("Inicializando RedisCacheConnection");

        var settingsNotNull = redisSettings == null;
        if (settingsNotNull)
        {
            throw new ArgumentNullException(nameof(redisSettings));
        }

        connectionMultiplexer = new Lazy<ConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisSettings.ConnectionString));
    }

    public IDatabase GetDatabase()
    {
        logger?.LogInformation($"Método: { nameof(GetDatabase) }() Retorno: IDatabase");

        return connectionMultiplexer.Value.GetDatabase();
    }
}