using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;

namespace Optsol.Components.Test.Utils.Data
{
    public class TestReadRepository : Repository<TestEntity, Guid>, ITestReadRepository
    {
        public TestReadRepository(CoreContext context, ILogger<Repository<TestEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }

    
}
