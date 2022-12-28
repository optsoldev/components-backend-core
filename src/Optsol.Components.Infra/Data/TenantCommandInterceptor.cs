using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Securities;

namespace Optsol.Components.Infra.Data;

public class TenantCommandInterceptor : DbCommandInterceptor
{
    private readonly ILoggedUser _loggedUser;
    
    public TenantCommandInterceptor(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }
}