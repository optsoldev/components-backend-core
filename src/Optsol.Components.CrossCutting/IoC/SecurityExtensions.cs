using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Optsol.Components.Infra.Security.Models;
using Optsol.Components.Infra.Security.Services;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using Refit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SecurityStoreExtensions
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = services.BuildServiceProvider();

            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>()
                ?? throw new SecuritySettingNullException(provider.GetRequiredService<ILoggerFactory>());

            securitySettings.Validate();

            services.AddSingleton(securitySettings);

            if (securitySettings.Development)
            {
                ConfigureLocalSecurity(services);
            }
            else
            {
                services
                    .AddRefitClient<AuthorityClient>()
                    .ConfigureHttpClient(config => config.BaseAddress = new Uri(securitySettings.Authority.Url));

                services.AddTransient<IAuthorityService, AuthorityService>();

                ConfigureRemoteSecurity(services, securitySettings);
            }

            return services;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app, IConfiguration configuration)
        {
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(nameof(UseSecurity));

            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>()
                ?? throw new SecuritySettingNullException(loggerFactory);
            securitySettings.Validate();

            app.UseAuthentication();
            app.UseAuthorization();

            if (securitySettings.Development)
            {
                IdentityModelEventSource.ShowPII = true;

                logger?.LogInformation("Configurando Segurança Local (IsDevelopment: true)");

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/token", async (context) =>
                    {
                        var symmetricSecurityKey = LocalSecuritySettings.GetSymmetricSecurityKey();
                        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            LocalSecuritySettings.Issuer,
                            LocalSecuritySettings.Audience,
                            GetLocalClaims(),
                            DateTime.Now,
                            DateTime.UtcNow.AddYears(1),
                            signingCredentials);

                        await context.Response.WriteAsync($"Token: { new JwtSecurityTokenHandler().WriteToken(token) }");
                    });
                });

            }
            else
            {
                logger?.LogInformation("Configurando Segurança Remota (IsDevelopment: false)");
            }

            return app;
        }

        internal static void ConfigureRemoteSecurity(IServiceCollection services, SecuritySettings securitySettings)
        {
            var provider = services.BuildServiceProvider();

            var clientOauth = provider.GetRequiredService<IAuthorityService>()
                .GetClient(securitySettings.Authority.ClientId)
                .GetAwaiter()
                .GetResult();

            if (clientOauth == null)
            {
                return;
            }

            var config = new ConfigurationBuilder();
            config.AddInMemoryObject(clientOauth, "AzureAdB2C");

            services.Configure<OauthClient>("AzureAdB2C", configure =>
            {
                configure = clientOauth;
            });

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryObject(clientOauth, "AzureAdB2C");
            var configurationLocal = configBuilder.Build();

            services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddMicrosoftIdentityWebApi(options =>
               {
                   configurationLocal.GetSection("AzureAdB2C").Bind(options);
                   options.TokenValidationParameters.NameClaimType = "name";
               }, options =>
               {
                   configurationLocal.GetSection("AzureAdB2C").Bind(options);
               });
        }

        internal static IServiceCollection ConfigureLocalSecurity(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = LocalSecuritySettings.Issuer,
                        ValidAudience = LocalSecuritySettings.Audience,

                        IssuerSigningKey = LocalSecuritySettings.GetSymmetricSecurityKey()
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = (context) =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                var loggerFactory = context.HttpContext.RequestServices
                                    .GetRequiredService<ILoggerFactory>();

                                var logger = loggerFactory.CreateLogger("Startup");

                                logger.LogInformation("Token-Expired");
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        },

                        OnMessageReceived = (context) =>
                        {
                            return Task.FromResult(0);
                        }
                    };
                });

            return services;
        }

        internal static class LocalSecuritySettings
        {
            public readonly static string Issuer = "issuer";
            public readonly static string Audience = "audience";
            public readonly static string Key = "optsol-security-key";

            public static SymmetricSecurityKey GetSymmetricSecurityKey()
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            }
        }

        public static Claim[] GetLocalClaims()
        {
            return new[]
            {
                new Claim(ClaimTypes.Name, "Optosol Local User"),
                new Claim(ClaimTypes.Email, "optsol.local@optsol.com.br"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("nonce", "635851137282360087.M2Y2NDM5NmUtMmRkYS00YWVjLTgwMWEtYWYwNDQyYzFmZmIxOTk0YjBlNmQtNjE5ZC00MDg5LWE2NTItNjc0OTJjNTU1Y2Rm"),
                new Claim("iat", "1449516934"),
                new Claim("at_hash", "vGWPYD4ZcS5RbJh6kPtwOw"),
                new Claim("http://schemas.microsoft.com/claims/authnmethodsreferences", "password"),
                new Claim("auth_time", "1449516934"),
                new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", "devtest"),
                new Claim("optsol", "cliente.buscar"),
                new Claim("optsol", "cliente.buscar.todos"),
                new Claim("optsol", "cliente.inserir")
            };
        }
    }

    public static class ObjectExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(this Object settings, string settingsRoot)
        {
            foreach (var property in settings.GetType().GetProperties())
            {
                yield return new KeyValuePair<string, string>($"{settingsRoot}:{property.Name}", property.GetValue(settings).ToString());
            }
        }
    }

    public static class ConfigurationBuilderExtensions
    {
        public static void AddInMemoryObject(this ConfigurationBuilder configurationBuilder, object settings, string settingsRoot)
        {
            configurationBuilder.AddInMemoryCollection(settings.ToKeyValuePairs(settingsRoot));
        }
    }
}