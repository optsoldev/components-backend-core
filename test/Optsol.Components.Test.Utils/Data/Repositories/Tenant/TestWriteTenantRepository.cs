using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Tenant
{
    public class TestTenantWriteRepository : Repository<TestTenantEntity, Guid>, ITestTenantWriteRepository
    {
        public TestTenantWriteRepository(CoreContext context, ILoggerFactory logger) 
            : base(context, logger)
        {
        }
    }
}
