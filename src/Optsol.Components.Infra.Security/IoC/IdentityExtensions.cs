using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Shared.Settings;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityExtensions
    {

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration, string migrationAssembly, Func<IServiceProvider, IConfigurationSecurityData> implementation)
        {
            var connectionStrings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>();
            connectionStrings.Validate();

            var migrationAssemblyIsNullOrEmpty = string.IsNullOrEmpty(migrationAssembly);
            if (migrationAssemblyIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(migrationAssembly));
            }

            services
                .AddSingleton(connectionStrings)
                .AddIdentityServer()
                .AddConfigurationStore(setup =>
                {
                    setup.ConfigureDbContext = context => context.UseSqlServer(connectionStrings.IdentityConnection, sql => sql.MigrationsAssembly(migrationAssembly));
                })
                .AddOperationalStore(setup =>
                {
                    setup.ConfigureDbContext = context => context.UseSqlServer(connectionStrings.IdentityConnection, sql => sql.MigrationsAssembly(migrationAssembly));
                });

            services.AddScoped(implementation);

            return services;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app)
        {
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
