using Optsol.Components.Infra.Security.Response;

namespace Optsol.Components.Infra.Security.Services
{
    public interface IUserService
    {
        ValidationResult Authenticate(string username, string password);
    }
}
