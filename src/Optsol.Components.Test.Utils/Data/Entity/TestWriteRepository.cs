using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using System;

namespace Optsol.Components.Test.Utils.Entity
{
    public class TestWriteRepository : Repository<TestEntity, Guid>, ITestWriteRepository
    {
        public TestWriteRepository(CoreContext context, ILogger<Repository<TestEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }


}
