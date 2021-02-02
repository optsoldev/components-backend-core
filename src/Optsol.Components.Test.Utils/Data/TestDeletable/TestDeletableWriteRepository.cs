using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;

namespace Optsol.Components.Test.Utils.Data
{
    public class TestDeletableWriteRepository : Repository<TestDeletableEntity, Guid>, ITestDeletableWriteRepository
    {
        public TestDeletableWriteRepository(CoreContext context, ILogger<Repository<TestDeletableEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }


}
