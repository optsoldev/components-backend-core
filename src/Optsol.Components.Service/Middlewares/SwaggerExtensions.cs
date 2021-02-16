using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Optsol.Components.Shared.Settings;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerExtensions
    {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder, IConfiguration configuration, bool isDevelopment)
        {
            var swaggerSettings = configuration.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
            swaggerSettings.Validate();

            var enabledSwagger = swaggerSettings.Enabled;
            if (enabledSwagger)
            {
                builder.UseSwagger();
                builder.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint($"/swagger/{swaggerSettings.Version}/swagger.json", $"{swaggerSettings.Name} {swaggerSettings.Version.ToUpper()}");

                    var enabledSecurity = swaggerSettings.Security?.Enabled ?? false;
                    if (enabledSecurity)
                    {
                        options.OAuthClientId("optsol-swagger");
                        options.OAuthAppName("Swagger UI for components optsol");
                        options.OAuthUsePkce();

                        if (isDevelopment)
                        {
                            options.OAuthClientSecret("secret".Sha256());
                        }
                    }
                });

            }

            return builder;
        }
    }
}
