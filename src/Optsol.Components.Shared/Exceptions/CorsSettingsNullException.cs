using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class CorsSettingsNullException : Exception
    {
        public CorsSettingsNullException(ILogger<CorsSettingsNullException> logger = null)
            : base("A configuração do CORS não foi encontrada no appsettings")
        {
            logger?.LogCritical(
@$"{nameof(CorsSettingsNullException)}:
""CorsSettings"": [
    {{
        ""Policy"": ""_nomePolicy"",
        ""Origins"": {{
            ""ReactFrontHttp"": ""http://example:port"",
            ""ReactFrontHttps"": ""http://example:port""
        }}
    }}
])"
            );
        }
    }
}
