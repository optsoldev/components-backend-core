using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Tenant
{
    public class TestTenantReadRepository : Repository<TestTenantEntity, Guid>, ITestTenantReadRepository
    {
        public TestTenantReadRepository(CoreContext context, ILogger<Repository<TestTenantEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }

    
}
