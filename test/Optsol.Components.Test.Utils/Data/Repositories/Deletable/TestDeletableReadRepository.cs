using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Deletable
{
    public class TestDeletableReadRepository : Repository<TestDeletableEntity, Guid>, ITestDeletableReadRepository
    {
        public TestDeletableReadRepository(CoreContext context, ILogger<Repository<TestDeletableEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }

    
}
