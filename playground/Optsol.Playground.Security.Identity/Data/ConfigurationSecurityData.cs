using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Optsol.Components.Infra.Security.Constants;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Infra.Security.Services;
using System;
using System.Collections.Generic;

namespace Optsol.Security.Identity.Data
{
    public class SecurityDataService : ISecurityDataService
    {
        public IList<ApiScope> GetScopesConfig()
        {
            return new List<ApiScope>
            {
                new ApiScope("webapi", "Scopes Web API"),
                new ApiScope(IdentityServerConstants.StandardScopes.OpenId)
            };
        }

        public IList<ApiResource> GetApiResourcesConfig()
        {
            return new List<ApiResource>
            {
                new ApiResource("webapi",  "Client Web API")
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

                    AllowOfflineAccess = true,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = {"https://localhost:5001/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"https://localhost:5001"},
                    AllowedScopes =
                    {
                        "webapi",
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                }
            };
        }

        public IList<ApplicationUser> GetUsersConfig()
        {
            var password = new PasswordHasher<ApplicationUser>();
            Func<ApplicationUser, string> setPassword = (user) => password.HashPassword(user, "secret");

            var optsolSubject = Guid.NewGuid();
            var basicSubject = Guid.NewGuid();

            var users = new List<ApplicationUser>
            {
                 new ApplicationUser
                 {
                     Id = optsolSubject,
                     UserName = "optsol",
                     NormalizedUserName = "optsol",
                     IsEnabled = true,
                     SecurityStamp = Guid.NewGuid().ToString(),
                     Claims = new List<IdentityUserClaim<Guid>>
                     {
                         new IdentityUserClaim<Guid> { ClaimType = "sub", ClaimValue = optsolSubject.ToString() },
                         new IdentityUserClaim<Guid> { ClaimType = $"{SecurityClaimTypes.Optsol}", ClaimValue = "cliente.buscar" },
                         new IdentityUserClaim<Guid> { ClaimType = $"{SecurityClaimTypes.Optsol}", ClaimValue = "cliente.inserir" },
                         new IdentityUserClaim<Guid> { ClaimType = $"{SecurityClaimTypes.Optsol}", ClaimValue = "cliente.remover" }
                     },
                 },
                 new ApplicationUser
                 {
                     Id = basicSubject,
                     UserName = "basic",
                     NormalizedUserName = "basic",
                     IsEnabled = true,
                     SecurityStamp = Guid.NewGuid().ToString(),
                     Claims = new List<IdentityUserClaim<Guid>>
                     {
                         new IdentityUserClaim<Guid> { ClaimType = "sub", ClaimValue = basicSubject.ToString() }
                     },
                 }
            };

            users.ForEach(f => f.PasswordHash = setPassword(f));

            return users;
        }
    }
}
