using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class SecuritySettingNullException: Exception
    {
        public SecuritySettingNullException(ILoggerFactory logger)
            : base("A configuração de segurança não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(SecuritySettingNullException));
            _logger?.LogCritical(
@$"{nameof(SecuritySettingNullException)}:
""SecuritySettings"": {{
    ""ApiName"": ""{{client-name}}"",
    ""Authority"": ""{{http(s)://authority:port}}"",
    ""IsDevelopment"": ""false|true"",
  }}
"
            );
        }
    }
}
