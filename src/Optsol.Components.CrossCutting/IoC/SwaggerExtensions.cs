using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Optsol.Components.Service.Filters;
using Optsol.Components.Shared.Settings;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
            swaggerSettings.Validate();

            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>();
            securitySettings.Validate();

            var enabledSwagger = swaggerSettings.Enabled;
            if (enabledSwagger)
            {
                services.AddSingleton(swaggerSettings);
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(swaggerSettings.Version,
                        new OpenApiInfo
                        {
                            Title = swaggerSettings.Title,
                            Version = swaggerSettings.Version,
                            Description = swaggerSettings.Description
                        });

                    var enabledSecurity = swaggerSettings.Security?.Enabled ?? false;
                    if (enabledSecurity)
                    {
                        swaggerSettings.Security.Validate();

                        options.AddSecurityDefinition(swaggerSettings.Security.Name,
                            new OpenApiSecurityScheme
                            {
                                Type = SecuritySchemeType.OAuth2,
                                Flows = new OpenApiOAuthFlows
                                {
                                    AuthorizationCode = new OpenApiOAuthFlow
                                    {
                                        AuthorizationUrl = new Uri($"{securitySettings.Authority}/connect/authorize"),
                                        TokenUrl = new Uri($"{securitySettings.Authority}/connect/token"),
                                        Scopes = swaggerSettings.Security.Scopes
                                    }
                                }
                            });
                    }

                    options.OperationFilter<AuthorizeCheckOperationFilter>();
                });
            }

            return services;
        }
    }
}
