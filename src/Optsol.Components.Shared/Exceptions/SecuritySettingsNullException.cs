using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class SecuritySettingNullException: Exception
    {
        public SecuritySettingNullException(ILogger<SecuritySettingNullException> logger = null)
            : base("A configuração de segurança não foi encontrada no appsettings")
        {
            logger?.LogCritical(
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
