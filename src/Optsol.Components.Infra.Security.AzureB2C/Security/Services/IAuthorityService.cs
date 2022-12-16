using Optsol.Components.Infra.Security.AzureB2C.Security.Models;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Infra.Security.AzureB2C.Security.Services
{
    public interface IAuthorityService
    {
        Task<OauthClient> GetClient(SecuritySettings securitySettings);
    }
}
