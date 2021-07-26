using Optsol.Components.Infra.Security.Models;
using Refit;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Security.Services
{
    public interface IAuthorityClient
    {
        [Get("/api/aplicacao/client")]
        Task<OauthClient> GetClient(string clientId);

        [Get("/api/usuario/token/info")]
        Task<UserInfo> GetUserInfo([Header("Authorization")] string authorization);

        [Get("/api/usuario/token/has-accesses")]
        Task<UserInfo> GetValidateAccess([Header("Authorization")] string authorization, [Query] string accessesToValidate);
    }
}
