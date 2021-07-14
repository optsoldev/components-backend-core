using Optsol.Components.Infra.Security.Models;
using Refit;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Security.Services
{
    public interface AuthorityClient
    {
        [Get("/api/aplicacao/client")]
        Task<OauthClient> GetClient(string clientId);

        [Get("/api/usuario/token/info")]
        Task<UserInfo> GetUserInfo([Header("Authorization")] string authorization);
    }
}
