using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using System;

namespace Optsol.Components.Test.Utils.Entity
{
    public class TestReadRepository : Repository<TestEntity, Guid>, ITestReadRepository
    {
        public TestReadRepository(CoreContext context, ILogger<Repository<TestEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }

    
}
