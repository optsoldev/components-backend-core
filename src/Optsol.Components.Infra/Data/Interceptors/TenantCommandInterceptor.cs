using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Optsol.Components.Infra.Data.Interceptors;

public class TenantCommandInterceptor<TKey> : DbCommandInterceptor
{
    private readonly ITenantProvider<TKey> tenantProvider;
    
    public TenantCommandInterceptor(ITenantProvider<TKey> tenantProvider)
    {
        this.tenantProvider = tenantProvider;
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
        var tenantId = tenantProvider.TenantId;
        
        if(tenantId is not null)
            command.CommandText = command.CommandText.Replace(InfraConstants.TenantId.ToLower(), tenantId.ToString());
    }
}



