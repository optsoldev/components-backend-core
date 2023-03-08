﻿using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Redis.Connections;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Redis.Services;

public class RedisCacheService : IRedisCacheService
{
    protected readonly ILogger _logger;
    protected readonly IDatabase _database;
    private readonly string[] ignoredProperties = { "notifications", "isvalid" };

    public RedisCacheService(RedisCacheConnection connection, ILoggerFactory logger)
    {
        _logger = logger?.CreateLogger(nameof(RedisCacheService));
        _logger?.LogInformation("Inicializando RedisCacheService");

        _database = connection?.GetDatabase() ?? throw new ArgumentNullException(nameof(connection));
    }

    public Task<T> ReadAsync<T>(string key) where T : class
    {
        var value = _database.StringGet(key);

        var cacheEmpty = !value.HasValue;
        if (cacheEmpty)
        {
            return Task.FromResult(default(T));
        }

        var castTyped = value.ToString().To<T>();

        return Task.FromResult(castTyped);
    }

    public Task SaveAsync<T>(KeyValuePair<string, T> data) where T : class
    {
        _database.StringSet(data.Key, data.Value.ToJson(ignoredProperties));

        return Task.CompletedTask;
    }

    public Task SaveAsync<T>(KeyValuePair<string, T> data, int expirationInMinutes) where T : class
    {
        _database.StringSet(data.Key, data.Value.ToJson(ignoredProperties), expiry: new TimeSpan(0, expirationInMinutes, 0));

        return Task.CompletedTask;
    }

    public Task DeleteAsync(string key)
    {
        _database.KeyDelete(key);

        return Task.CompletedTask;
    }
}