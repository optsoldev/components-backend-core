using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Core
{
    public class TestWriteRepository : Repository<TestEntity, Guid>, ITestWriteRepository
    {
        public TestWriteRepository(CoreContext context, ILoggerFactory logger) 
            : base(context, logger)
        {
        }
    }


}
