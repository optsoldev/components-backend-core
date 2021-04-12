using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.Data.Provider;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Tenant
{
    public class TestTenantReadRepository : TenantRepository<TestTenantEntity, Guid>, ITestTenantReadRepository
    {
        public TestTenantReadRepository(CoreContext context, ILoggerFactory logger, ITenantProvider tenantProvider) 
            : base(context, logger, tenantProvider)
        {
        }
    }

    public class TestTenantDeletableReadRepository : TenantRepository<TestTenantDeletableEntity, Guid>, ITestTenantDeletableReadRepository
    {
        public TestTenantDeletableReadRepository(CoreContext context, ILoggerFactory logger, ITenantProvider tenantProvider)
            : base(context, logger, tenantProvider)
        {
        }
    }
}
