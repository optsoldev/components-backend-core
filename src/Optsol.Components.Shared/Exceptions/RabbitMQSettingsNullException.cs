using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class RabbitMQSettingsNullException : Exception
    {
        public RabbitMQSettingsNullException(ILoggerFactory logger = null)
            : base("A configuração do RabbitMQ não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(RabbitMQSettingsNullException));
            _logger?.LogCritical(
@$"{nameof(RabbitMQSettingsNullException)}:
""RabbitMQSettings"": {{
    ""HostName"": ""{{host}}"",
    ""Port"": ""{{5672}}"",
    ""UserName"": ""{{username}}""
    ""Password"": ""{{Password}}""
}}"
            );
        }
    }
}
