using IdentityServer4.Models;
using Optsol.Components.Infra.Security.Data;
using System.Collections.Generic;

namespace Optsol.Components.Infra.Security.Services
{
    public interface ISecurityDataService
    {
        IList<Client> GetClientsConfig();

        IList<ApiScope> GetScopesConfig();

        IList<ApiResource> GetApiResourcesConfig();

        IList<ApplicationUser> GetUsersConfig();
    }
}
