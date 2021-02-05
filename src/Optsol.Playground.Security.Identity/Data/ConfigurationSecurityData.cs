using IdentityServer4.Models;
using Optsol.Components.Infra.Security.Data;
using System.Collections.Generic;

namespace Optsol.Security.Identity.Data
{
    public class ConfigurationSecurityData : IConfigurationSecurityData
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
                    ClientId = "optsol-client",
                    RequirePkce = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = new List<string> { GrantType.AuthorizationCode },
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "webapi", "write", "read" },
                    Claims = new List<ClientClaim>
                    {
                        new ClientClaim("companyName", "John Doe LTD")
                    },
                    RedirectUris = new List<string>
                    {
                        "https://localhost:5001/signin-oidc"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:5001",
                    },
                    AccessTokenLifetime = 86400
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
