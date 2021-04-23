using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public sealed class SwaggerSettingsNullException : Exception
    {
        public SwaggerSettingsNullException(ILoggerFactory logger = null)
            : base("A configuração de segurança não foi encontrada no appsettings")
        {
            var _logger = logger.CreateLogger(nameof(SwaggerSettingsNullException));
            _logger?.LogCritical(
@$"{nameof(SwaggerSettingsNullException)}:
""SwaggerSettings"": {{
    ""Title"": ""{{title}}"",
    ""Name"": ""{{name your swagger}}"",
    ""Enabled"": ""true"",
    ""Version"": ""v1"",
    ""Description"": ""{{your description}}"",
    ""Security"": ""{{
        ""Name"": ""{{OAUTH Name}}"",
        ""Enabled"": ""{{true}}"",
        ""Scopes"": ""{{
            {{<key=name scope>}}:{{<value=description>}}
        }}"",
    }}""
  }}
");
        }
    }
}
