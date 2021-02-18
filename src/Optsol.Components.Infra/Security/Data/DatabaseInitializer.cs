using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.Security.Services;
using Optsol.Components.Shared.Exceptions;
using System.Linq;

namespace Optsol.Components.Infra.Security.Data
{
    public static class DatabaseInitializer
    {
        public static void PopulateIdentityServer(IApplicationBuilder app)
        {
            var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var persistedGrantContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                MigratePersistedGrant(persistedGrantContext);

                var configurationSecurityData = serviceScope.ServiceProvider.GetRequiredService<ISecurityDataService>() ?? throw new ConfigurationSecurityDataException<ISecurityDataService>();

                var configurationContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                MigrateConfiguration(configurationContext, configurationSecurityData);

                var userContext = serviceScope.ServiceProvider.GetRequiredService<SecurityDbContext>();
                MigrateUser(userContext, configurationSecurityData);
            }
        }

        private static void MigrateUser(SecurityDbContext context, ISecurityDataService configurationSecurityData)
        {
            context.Database.Migrate();

            var usersIsEmpty = !context.Users.Any();
            if (usersIsEmpty)
            {
                context.Users.AddRange(configurationSecurityData.GetUsersConfig());
                context.SaveChanges();
            }
        }

        private static void MigratePersistedGrant(PersistedGrantDbContext persistedGrantContext)
        {
            persistedGrantContext.Database.Migrate();
        }

        private static void MigrateConfiguration(ConfigurationDbContext context, ISecurityDataService configurationSecurityData)
        {
            context.Database.Migrate();

            var clientsIsEmpty = !context.Clients.Any();
            if (clientsIsEmpty)
            {
                context.Clients.AddRange(configurationSecurityData.GetClientsConfig().ToEntity());
                context.SaveChanges();
            }

            var apiResourcesIsEmpty = !context.ApiResources.Any();
            if (apiResourcesIsEmpty)
            {
                context.ApiResources.AddRange(configurationSecurityData.GetApiResourcesConfig().ToEntity());
                context.SaveChanges();
            }

            var apiScopesIsEmpty = !context.ApiScopes.Any();
            if (apiScopesIsEmpty)
            {
                context.ApiScopes.AddRange(configurationSecurityData.GetScopesConfig().ToEntity());
                context.SaveChanges();
            }
        }
    }
}
