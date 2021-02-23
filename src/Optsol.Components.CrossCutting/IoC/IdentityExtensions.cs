using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;

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

            services
                .AddSingleton(securitySettings)
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.ApiName = securitySettings.ApiName;
                    options.Authority = securitySettings.Authority;
                })
                .AddCookie();

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


            if (securityOptions != null)
                securityOptions(configSecurityOptions);

            return services;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseIdentityServer();

            return app;
        }

        public static IApplicationBuilder ConfigureSecurity(this IApplicationBuilder app)
        {
            DatabaseInitializer.PopulateIdentityServer(app);

            return app;
        }
    }
}
