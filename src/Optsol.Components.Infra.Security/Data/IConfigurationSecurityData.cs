using IdentityServer4.Models;
using System.Collections.Generic;

namespace Optsol.Components.Infra.Security.Data
{
    public interface IConfigurationSecurityData
    {
        IList<Client> GetClientsConfig();

        IList<ApiScope> GetScopesConfig();

        IList<ApiResource> GetApiResourcesConfig();
    }
}
