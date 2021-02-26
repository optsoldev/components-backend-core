using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var servicesProvider = services.BuildServiceProvider();

            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>()
                ?? throw new SecuritySettingNullException(servicesProvider.GetRequiredService<ILogger<SecuritySettingNullException>>());

            securitySettings.Validate();

            var connectionStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>()
                ?? throw new ConnectionStringNullException(servicesProvider.GetRequiredService<ILogger<ConnectionStringNullException>>());

            connectionStrings.Validate();

            services.AddSingleton(securitySettings);

            if (securitySettings.IsDevelopment)
            {
                ConfigureLocalSecurity(services);
            }
            else
            {
                ConfigureRemoteSecurity(services, securitySettings);
            }

            services.AddSingleton(connectionStrings);

            services.AddUserStore(options =>
            {
                options.ConfigureDbContext = context => context.UseSqlServer(connectionStrings.IdentityConnection);
            });

            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration, string migrationAssembly, bool isDevelopment, Action<ConfigSecurityOptions> securityOptions = null)
        {
            IdentityModel.Logging.IdentityModelEventSource.ShowPII = isDevelopment;

            var servicesProvider = services.BuildServiceProvider();

            var connectionStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>()
                ?? throw new ConnectionStringNullException(servicesProvider.GetRequiredService<ILogger<ConnectionStringNullException>>());
            connectionStrings.Validate();

            var migrationAssemblyIsNullOrEmpty = string.IsNullOrEmpty(migrationAssembly);
            if (migrationAssemblyIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(migrationAssembly));
            }

            services.AddSingleton(connectionStrings);

            var identityBuilder = services.AddIdentityServer();
            var configSecurityOptions = new ConfigSecurityOptions(services, identityBuilder);

            identityBuilder
                .AddDeveloperSigningCredential(isDevelopment)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = context => context.UseSqlServer(connectionStrings.IdentityConnection, sql => sql.MigrationsAssembly(migrationAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = context => context.UseSqlServer(connectionStrings.IdentityConnection, sql => sql.MigrationsAssembly(migrationAssembly));
                })
                .AddUserStore(options =>
                {
                    options.ConfigureDbContext = context => context.UseSqlServer(connectionStrings.IdentityConnection, sql => sql.MigrationsAssembly(migrationAssembly));
                });


            securityOptions?.Invoke(configSecurityOptions);

            return services;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app, IConfiguration configuration)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILogger<SecuritySettingNullException>>();

            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>() ?? throw new SecuritySettingNullException(logger);

            securitySettings.Validate();

            app.UseAuthentication();

            app.UseAuthorization();

            if (securitySettings.IsDevelopment)
            {
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

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app, bool enableMigrationIdentity = true)
        {
            if (enableMigrationIdentity)
            {
                DatabaseInitializer.PopulateIdentityServer(app);
            }

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseIdentityServer();

            return app;
        }

        #region private

        private static class LocalSecuritySettings
        {
            public static string Issuer = "issuer";
            public static string Audience = "audience";
            public static string Key = "optsol-security-key";

            public static SymmetricSecurityKey GetSymmetricSecurityKey()
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            }
        }

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
                new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", "devtest")
            };
        }

        private static IServiceCollection ConfigureLocalSecurity(this IServiceCollection services)
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

        private static IServiceCollection ConfigureRemoteSecurity(this IServiceCollection services, SecuritySettings securitySettings)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.ApiName = securitySettings.ApiName;
                    options.Authority = securitySettings.Authority;
                })
                .AddCookie();

            return services;

        }

        #endregion
    }
}
