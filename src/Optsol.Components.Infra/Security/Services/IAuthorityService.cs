using Optsol.Components.Infra.Security.Models;
using Optsol.Components.Shared.Settings;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Security.Services
{
    public interface IAuthorityService
    {
        Task<OauthClient> GetClient(SecuritySettings securitySettings);
    }
}
