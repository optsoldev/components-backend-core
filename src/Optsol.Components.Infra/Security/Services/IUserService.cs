using Optsol.Components.Infra.Security.Response;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Security.Services
{
    public interface IUserService
    {
        Task<ValidationResult> Authenticate(string username, string password);
    }
}
