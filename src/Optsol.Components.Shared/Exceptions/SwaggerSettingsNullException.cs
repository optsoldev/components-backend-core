﻿using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class SwaggerSettingsNullException : Exception
    {
        public SwaggerSettingsNullException(ILogger<SwaggerSettingsNullException> logger = null)
            : base("A configuração de segurança não foi encontrada no appsettings")
        {
            logger?.LogCritical(
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