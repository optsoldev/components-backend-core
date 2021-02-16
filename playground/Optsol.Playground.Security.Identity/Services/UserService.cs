using Optsol.Components.Infra.Security.Response;
using Optsol.Components.Infra.Security.Services;
using System.Linq;

namespace Optsol.Playground.Security.Identity.Services
{
    using static Optsol.Components.Infra.Security.Development.Confg;

    public class UserService : IUserService
    {
        public ValidationResult Authenticate(string username, string password)
        {
            var user = GetUsers().FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return ValidationResult.Failure();
            }

            return string.CompareOrdinal(user.Password, password) == 0
                ? ValidationResult.Success(user.SubjectId)
                : ValidationResult.Failure();
        }
    }
}
