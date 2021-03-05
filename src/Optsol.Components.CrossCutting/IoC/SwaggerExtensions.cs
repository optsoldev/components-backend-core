using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Optsol.Components.Service.Filters;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;
using System.Collections.Generic;

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
                        swaggerSettings.Security.Validate();

                        if (securitySettings.IsDevelopment)
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
{                               {
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

                            //options.OperationFilter<ApiKeyOperationFilter>();
                        }
                        else
                        {
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
                                
                            options.OperationFilter<AuthorizeCheckOperationFilter>();
                        }

                    }
                });
            }

            return services;
        }
    }
}
