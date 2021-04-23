using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class RedisSettingsNullException : Exception
    {
        public RedisSettingsNullException(ILoggerFactory logger = null)
            : base("A configuração do REDIS não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(RedisSettingsNullException));
            _logger?.LogCritical(
@$"{nameof(RedisSettingsNullException)}:
""RedisSettings"": {{
    ""ConnectionString"": ""...redis.cache.windows.net,abortConnect=false,ssl=true...""
}}"
            );
        }
    }
}
