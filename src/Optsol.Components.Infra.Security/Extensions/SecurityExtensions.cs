using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Entities = IdentityServer4.EntityFramework.Entities;

namespace Optsol.Components.Infra.Security.Data
{
    public static class SecurityExtensions
    {
        public static IEnumerable<Entities.Client> ToEntity(this IList<Client> clients)
        {
            foreach (var client in clients)
                yield return client.ToEntity();
        }

        public static IEnumerable<Entities.ApiResource> ToEntity(this IList<ApiResource> clients)
        {
            foreach (var client in clients)
                yield return client.ToEntity();
        }

        public static IEnumerable<Entities.ApiScope> ToEntity(this IList<ApiScope> clients)
        {
            foreach (var client in clients)
                yield return client.ToEntity();
        }

        public static IIdentityServerBuilder AddTestUsers(this IIdentityServerBuilder builder, bool enabled, List<TestUser> users)
        {
            if (enabled)
            {
                builder.AddTestUsers(users);
            }

            return builder;
        }
    }
}
