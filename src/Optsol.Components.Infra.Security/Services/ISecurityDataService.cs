using IdentityServer4.Models;
using System.Collections.Generic;

namespace Optsol.Components.Infra.Security.Services
{
    public interface ISecurityDataService
    {
        IList<Client> GetClientsConfig();

        IList<ApiScope> GetScopesConfig();

        IList<ApiResource> GetApiResourcesConfig();
    }
}
