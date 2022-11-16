using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Optsol.Components.Infra.Security.Services;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;
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

                        if (securitySettings.Development)
                        {
                            ConfigureDevelopmentSecurity(options);
                        }
                        else
                        {
                            ConfigureRemoteSecurity(provider.GetRequiredService<IAuthorityService>(), swaggerSettings, options);
                        }

                    }
                });
            }

            return services;
        }

        private static void ConfigureRemoteSecurity(IAuthorityService authorityService, SwaggerSettings swaggerSettings, SwaggerGenOptions options)
        {
            //var clientOauth = authorityService
            //    .GetClient(swaggerSettings.Security.ClientId)
            //    .GetAwaiter()
            //    .GetResult();

            //if (clientOauth == null)
            //{
            //    return;
            //}

            //var scopes = swaggerSettings
            //    .Security
            //    .Scopes
            //    .Select(scope => new KeyValuePair<string, string>($"https://{clientOauth.Domain}/{scope.Key}/{scope.Value}", $"{scope.Key}"));

            //var endpointB2C = $"{clientOauth.Instance}/{clientOauth.Domain}/{clientOauth.SignUpSignInPolicyId}/oauth2/v2.0";

            //options.AddSecurityDefinition(swaggerSettings.Security.Name,
            //    new OpenApiSecurityScheme
            //    {
            //        Type = SecuritySchemeType.OAuth2,
            //        Flows = new OpenApiOAuthFlows
            //        {
            //            Implicit = new OpenApiOAuthFlow()
            //            {
            //                AuthorizationUrl = new Uri($"{endpointB2C}/authorize"),
            //                TokenUrl = new Uri($"{endpointB2C}/token"),
            //                Scopes = new Dictionary<string, string>(scopes)
            //            },
            //        }
            //    });

            //options.OperationFilter<AuthorizeCheckOperationFilter>();
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
                        options.OAuthAppName($"Swagger UI for {swaggerSettings.Security.Name}");
                        options.OAuthClientId(swaggerSettings.Security.ClientId);

                        if (isDevelopment)
                        {
                            options.OAuthClientSecret("secret".Sha256());

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
