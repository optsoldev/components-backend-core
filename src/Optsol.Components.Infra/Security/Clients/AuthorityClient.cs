using Optsol.Components.Infra.Security.Models;
using Refit;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Security.Services
{
    public interface AuthorityClient
    {
        [Get("/api/client")]
        Task<OauthClient> GetClient(string clientId);
    }
}
