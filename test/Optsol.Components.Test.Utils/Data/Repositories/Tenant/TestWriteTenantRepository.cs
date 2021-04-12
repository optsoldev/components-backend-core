using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.Data.Provider;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Tenant
{
    public class TestTenantWriteRepository : TenantRepository<TestTenantEntity, Guid>, ITestTenantWriteRepository
    {
        public TestTenantWriteRepository(CoreContext context, ILoggerFactory logger, ITenantProvider tenantProvider) 
            : base(context, logger, tenantProvider)
        {
        }
    }
}
