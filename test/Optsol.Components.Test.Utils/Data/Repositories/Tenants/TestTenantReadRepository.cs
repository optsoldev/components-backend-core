using System;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Data.Repositories.Tenants
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
