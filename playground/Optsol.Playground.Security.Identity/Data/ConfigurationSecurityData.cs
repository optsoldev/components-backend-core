using IdentityServer4.Models;
using Optsol.Components.Infra.Security.Services;
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
    }
}
