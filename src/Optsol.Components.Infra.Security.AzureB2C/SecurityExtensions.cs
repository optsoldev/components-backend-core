using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Shared.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Optsol.Components.Infra.Security.AzureB2C.Security.Models;
using Optsol.Components.Infra.Security.AzureB2C.Security.Services;

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
                services.ConfigureLocalSecurity();
            }
            else
            {
                services.AddRemoteSecurity(securitySettings);
                var configInMemory = services.GetRemoteConfiguration(securitySettings);
                services.ConfigureRemoteSecurity(configInMemory);
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
                logger?.LogInformation("Configurando Segurança Local (IsDevelopment: true)");

                app.UseRemoteEndpoint();
            }
            else
            {
                logger?.LogInformation("Configurando Segurança Remota (IsDevelopment: false)");
            }

            return app;
        }
    }

    public static class ConfigurationBuilderExtensions
    {
        public static void AddInMemoryObject(this ConfigurationBuilder configurationBuilder, object settings, string settingsRoot)
        {
            configurationBuilder.AddInMemoryCollection(settings.ToKeyValuePairs(settingsRoot));
        }       
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRemoteSecurity(this IServiceCollection services, SecuritySettings securitySettings)
        {
            //services
            //    .AddRefitClient<IAuthorityClient>()
            //    .ConfigureHttpClient(config => config.BaseAddress = new Uri(securitySettings.Authority.Endpoint));

            services.AddTransient<IAuthorityService, AuthorityService>();

            return services;
        }

        public static IConfiguration GetRemoteConfiguration(this IServiceCollection services, SecuritySettings securitySettings)
        {
            var provider = services.BuildServiceProvider();

            //var clientOauth = provider.GetRequiredService<IAuthorityService>()
            //    .GetClient(securitySettings.Authority.ClientId)
            //    .GetAwaiter()
            //    .GetResult();

            //if (clientOauth == null)
            //{
            //    return null;
            //}

            OauthClient clientOauth = CreateOauthClient(securitySettings);

            var config = new ConfigurationBuilder();
            config.AddInMemoryObject(clientOauth, "AzureAdB2C");

            services.Configure<OauthClient>("AzureAdB2C", configure =>
            {
                configure = clientOauth;
            });

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryObject(clientOauth, "AzureAdB2C");
            return configBuilder.Build();
        }

        public static IServiceCollection ConfigureRemoteSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationIsNull = configuration == null;
            if (configurationIsNull)
            {
                return services;
            }

            services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddMicrosoftIdentityWebApi(options =>
               {
                   configuration?.GetSection("AzureAdB2C").Bind(options);
                   options.TokenValidationParameters.NameClaimType = "name";
               }, options =>
               {
                   configuration?.GetSection("AzureAdB2C").Bind(options);
               });


            return services;
        }

        public static IServiceCollection ConfigureLocalSecurity(this IServiceCollection services)
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

        public static OauthClient CreateOauthClient(SecuritySettings securitySettings)
        {
            return new OauthClient
            {
                Instance = securitySettings.Authority.Instance,
                ClientId = securitySettings.Authority.ClientId,
                Domain = securitySettings.Authority.Domain,
                SignedOutCallbackPath = "/signout/B2C_1_login",
                SignUpSignInPolicyId = "b2c_1_login",
                ResetPasswordPolicyId = "b2c_1_reset",
                EditProfilePolicyId = "b2c_1_edit",
            };
        }
    }

    public static class ApplicationBuilderExtensions
    {
        private static Claim[] GetLocalClaims()
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

        public static IApplicationBuilder UseRemoteEndpoint(this IApplicationBuilder app)
        {
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

            return app;
        }
    }

    public static class LocalSecuritySettings
    {
        public readonly static string Issuer = "issuer";
        public readonly static string Audience = "audience";
        public readonly static string Key = "optsol-security-key";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }
    }
}