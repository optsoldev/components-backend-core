using Optsol.Components.Infra.Security.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Security.Services
{
    public interface IAuthorityService
    {
        Task<OauthClient> GetClient(string clientId);

        Task<UserInfo> GetUserInfo(string token);

        Task<UserInfo> GetValidateAccess(string token, IList<string> claims);
    }
}
