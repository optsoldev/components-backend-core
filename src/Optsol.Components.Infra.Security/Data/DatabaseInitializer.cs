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
                var configurationSecurityData = serviceScope.ServiceProvider.GetRequiredService<ISecurityDataService>() ?? throw new ConfigurationSecurityDataException<ISecurityDataService>();

                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
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
                if(apiScopesIsEmpty)
                {
                    context.ApiScopes.AddRange(configurationSecurityData.GetScopesConfig().ToEntity());
                    context.SaveChanges();
                }
            }
        }
    }
}
