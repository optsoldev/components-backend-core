using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Optsol.Components.Shared.Settings;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerExtensions
    {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration, bool isDevelopment)
        {
            var swaggerSettings = configuration.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
            swaggerSettings.Validate();

            var enabledSwagger = swaggerSettings.Enabled;
            if (enabledSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint($"/swagger/{swaggerSettings.Version}/swagger.json", $"{swaggerSettings.Name} {swaggerSettings.Version.ToUpper()}");

                    var enabledSecurity = swaggerSettings.Security?.Enabled ?? false;
                    if (enabledSecurity)
                    {
                        options.OAuthClientId("436453d1-adf6-4160-93f9-9e665fc94d65");
                        options.OAuthAppName("Swagger UI for components optsol");
                        
                        if (isDevelopment)
                        {
                            options.OAuthClientSecret("secret".Sha256());
                        }

                        if (isDevelopment)
                        {
                            app.Use(async (context, next) =>
                            {
                                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                                context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                                await next();
                            });
                        }
                    }
                });
            }

            return app;
        }
    }
}
