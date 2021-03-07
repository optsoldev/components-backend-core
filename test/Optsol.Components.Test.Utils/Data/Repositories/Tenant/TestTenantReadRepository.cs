using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Tenant
{
    public class TestTenantReadRepository : Repository<TestTenantEntity, Guid>, ITestTenantReadRepository
    {
        public TestTenantReadRepository(CoreContext context, ILoggerFactory logger) 
            : base(context, logger)
        {
        }
    }

    public class TestTenantDeletableReadRepository : Repository<TestTenantDeletableEntity, Guid>, ITestTenantDeletableReadRepository
    {
        public TestTenantDeletableReadRepository(CoreContext context, ILoggerFactory logger)
            : base(context, logger)
        {
        }
    }
}
