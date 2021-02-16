﻿using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Infra.Security.Development;
using Optsol.Components.Infra.Security.Services;
using Optsol.Components.Shared.Settings;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>();
            securitySettings.Validate();

            services
                .AddSingleton(securitySettings)
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.ApiName = securitySettings.ApiName;
                    options.Authority = securitySettings.Authority;
                })
                .AddCookie();

            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration, string migrationAssembly, bool isDevelopment)
        {
            IdentityModel.Logging.IdentityModelEventSource.ShowPII = isDevelopment;

            var connectionStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>() ?? throw new ArgumentNullException(nameof(ConnectionStrings));
            connectionStrings.Validate();

            var migrationAssemblyIsNullOrEmpty = string.IsNullOrEmpty(migrationAssembly);
            if (migrationAssemblyIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(migrationAssembly));
            }

            services
                .AddSingleton(connectionStrings)
                .AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = context => context.UseSqlServer(connectionStrings.IdentityConnection, sql => sql.MigrationsAssembly(migrationAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = context => context.UseSqlServer(connectionStrings.IdentityConnection, sql => sql.MigrationsAssembly(migrationAssembly));
                })
                .AddDeveloperSigningCredential(isDevelopment)
                .AddTestUsers(isDevelopment, Confg.GetUsers());


            return services;
        }

        public static IServiceCollection AddSecurity<TSecurityData, TUserService>(this IServiceCollection services, IConfiguration configuration, string migrationAssembly, bool enableTestUser)
            where TSecurityData : IConfigurationSecurityData
            where TUserService : IUserService
        {
            AddSecurity(services, configuration, migrationAssembly, enableTestUser);

            services.AddScoped(typeof(IConfigurationSecurityData), typeof(TSecurityData));
            services.AddTransient(typeof(IUserService), typeof(TUserService));

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
