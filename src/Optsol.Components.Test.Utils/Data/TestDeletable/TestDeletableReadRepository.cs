using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;

namespace Optsol.Components.Test.Utils.Data
{
    public class TestDeletableReadRepository : Repository<TestDeletableEntity, Guid>, ITestDeletableReadRepository
    {
        public TestDeletableReadRepository(DbContext context, ILogger<Repository<TestDeletableEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }

    
}
