using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Infra.Security.Services;
using System;
using System.Collections.Generic;

namespace Optsol.Security.Identity.Data
{
    public class SecurityDataService : ISecurityDataService
    {

        public IList<ApiResource> GetApiResourcesConfig()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "webapi",
                    DisplayName = "API Security",
                    Scopes = new List<string>
                    {
                        "write",
                        "read"
                    }
                }
            };
        }

        public IList<Client> GetClientsConfig()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "optsol-swagger",
                    ClientName = "Swagger UI for components optsol",
                    ClientSecrets = {new Secret("secret".Sha256())},

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = {"https://localhost:5001/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"https://localhost:5001"},
                    AllowedScopes = { "webapi", "write", "read" },
                }
            };
        }

        public IList<ApiScope> GetScopesConfig()
        {
            return new List<ApiScope>
            {
                new ApiScope("read"),
                new ApiScope("write")
            };
        }

        public IList<ApplicationUser> GetUsersConfig()
        {
            var password = new PasswordHasher<ApplicationUser>();
            Func<ApplicationUser, string> setPassword = (user) => password.HashPassword(user, "secret");

            var users = new List<ApplicationUser>
            {
                 new ApplicationUser
                 {
                     Id = Guid.NewGuid(),
                     UserName = "optsol",
                     NormalizedUserName = "optsol",
                     ExternalId = Guid.NewGuid(),
                     IsEnabled = true,
                     SecurityStamp = Guid.NewGuid().ToString(),
                     Claims = new List<IdentityUserClaim<Guid>>
                     {
                         new IdentityUserClaim<Guid> { ClaimType = "sub", ClaimValue = "1" },
                         new IdentityUserClaim<Guid> { ClaimType = "optsol", ClaimValue = "crud.buscar.id" },
                         new IdentityUserClaim<Guid> { ClaimType = "optsol", ClaimValue = "cliente.buscar.id" },
                         new IdentityUserClaim<Guid> { ClaimType = "optsol", ClaimValue = "cliente.buscar.todos" }
                     },
                 },
                 new ApplicationUser
                 {
                     Id = Guid.NewGuid(),
                     UserName = "basic",
                     NormalizedUserName = "basic",
                     ExternalId = Guid.NewGuid(),
                     IsEnabled = true,
                     SecurityStamp = Guid.NewGuid().ToString(),
                     Claims = new List<IdentityUserClaim<Guid>>
                     {
                         new IdentityUserClaim<Guid> { ClaimType = "sub", ClaimValue = "2" }
                     },
                 }
            };

            users.ForEach(f => f.PasswordHash = setPassword(f));

            return users;
        }
    }
}
