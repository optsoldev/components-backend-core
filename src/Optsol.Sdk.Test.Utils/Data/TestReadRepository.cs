using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Sdk.Infra.Data;
using Optsol.Sdk.Test.Shared.Data;

namespace Optsol.Sdk.Test.Utils.Data
{
    public class TestReadRepository : Repository<TestEntity, Guid>, ITestReadRepository
    {
        public TestReadRepository(DbContext context, ILogger<Repository<TestEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }

    
}
