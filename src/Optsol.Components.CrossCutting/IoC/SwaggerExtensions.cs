using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Optsol.Components.Service.Filters;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = services.BuildServiceProvider();

            var swaggerSettings = configuration.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>()
                ?? throw new SwaggerSettingsNullException(provider.GetRequiredService<ILoggerFactory>());

            swaggerSettings.Validate();

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
                        var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>()
                            ?? throw new SecuritySettingNullException(provider.GetRequiredService<ILoggerFactory>());

                        securitySettings.Validate();

                        if (securitySettings.IsDevelopment)
                        {
                            ConfigureDevelopmentSecurity(options);
                        }
                        else
                        {
                            ConfigureRemoteSecurity(options, securitySettings, swaggerSettings);
                        }

                    }
                });
            }

            return services;
        }

        private static void ConfigureRemoteSecurity(SwaggerGenOptions options, SecuritySettings securitySettings, SwaggerSettings swaggerSettings)
        {
            var scopes = swaggerSettings
                .Security
                .Scopes
                .Select(scope => new KeyValuePair<string, string>($"https://{securitySettings.AzureB2C.Domain}/{scope.Key}/{scope.Value}", $"{scope.Key}"));

            var endpointB2C = $"{securitySettings.AzureB2C.Instance}/{securitySettings.AzureB2C.Domain}/{securitySettings.AzureB2C.SignUpSignInPolicyId}/oauth2/v2.0";

            options.AddSecurityDefinition(swaggerSettings.Security.Name,
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            Scopes = new Dictionary<string, string>(scopes),
                            AuthorizationUrl = new Uri($"{endpointB2C}/authorize"),
                            TokenUrl = new Uri($"{endpointB2C}/token")
                        },
                    }
                });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
        }

        private static void ConfigureDevelopmentSecurity(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef.23fdeeecxxXE...\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
{               {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        }
    }
}
