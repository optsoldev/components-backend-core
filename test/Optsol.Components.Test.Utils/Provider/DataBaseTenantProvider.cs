using Microsoft.AspNetCore.Http;
using Optsol.Components.Infra.Data.Provider;
using Optsol.Components.Test.Utils.Data.Contexts;
using System;
using System.Linq;

namespace Optsol.Components.Test.Utils.Provider
{
    public class DataBaseTenantProvider : ITenantProvider
    {
        private Guid _tenantId;

        public DataBaseTenantProvider(TenantDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            var host = httpContextAccessor.HttpContext?.Request.Host.Value;
            if (!string.IsNullOrEmpty(host))
            {
                _tenantId = context.Tenants.First(f => f.Host.Equals(host)).Id;
            }
        }

        public Guid GetTenantId()
        {
            return _tenantId;
        }
    }
}
