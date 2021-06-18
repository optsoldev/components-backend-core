using Optsol.Components.Infra.Security.Models;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Security.Services
{
    public interface IAuthorityService
    {
        Task<OauthClient> GetClient(string clientId);
    }
}
