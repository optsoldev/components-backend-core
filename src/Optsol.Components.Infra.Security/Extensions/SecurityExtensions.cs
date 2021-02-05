using IdentityServer4.EntityFramework.Mappers;
using System.Collections.Generic;
using Entities = IdentityServer4.EntityFramework.Entities;
using Models = IdentityServer4.Models;

namespace Optsol.Components.Infra.Security.Data
{
    public static class SecurityExtensions
    {
        public static IEnumerable<Entities.Client> ToEntity(this IList<Models.Client> clients)
        {
            foreach (var client in clients)
                yield return client.ToEntity();
        }

        public static IEnumerable<Entities.ApiResource> ToEntity(this IList<Models.ApiResource> clients)
        {
            foreach (var client in clients)
                yield return client.ToEntity();
        }

        public static IEnumerable<Entities.ApiScope> ToEntity(this IList<Models.ApiScope> clients)
        {
            foreach (var client in clients)
                yield return client.ToEntity();
        }
    }
}
