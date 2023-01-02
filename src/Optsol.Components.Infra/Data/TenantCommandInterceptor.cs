using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Securities;

namespace Optsol.Components.Infra.Data;

public class TenantCommandInterceptor<TKey> : DbCommandInterceptor
{
    private readonly ILoggedUser<TKey> loggedUser;
    
    public TenantCommandInterceptor(ILoggedUser<TKey> loggedUser)
    {
        this.loggedUser = loggedUser;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        ModifyCommand(command);
        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = new())
    {
        ModifyCommand(command);
        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private void ModifyCommand(IDbCommand command)
    {
        var tenantId = loggedUser.GetTenantId();
        if(tenantId is not null)
            command.CommandText = command.CommandText.Replace(InfraConstants.TenantId.ToLower(), tenantId.ToString());
    }
}



