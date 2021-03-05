using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class CorsSettingsNullException : Exception
    {
        public CorsSettingsNullException(ILoggerFactory logger = null)
            : base("A configuração do CORS não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(CorsSettingsNullException));
            _logger?.LogCritical(
@$"{nameof(CorsSettingsNullException)}:
""CorsSettings"": [
    {{
        ""Policy"": ""_nomePolicy"",
        ""Origins"": {{
            ""ReactFrontHttp"": ""http://example:port"",
            ""ReactFrontHttps"": ""http://example:port""
        }}
    }}
]"
            );
        }
    }
}
