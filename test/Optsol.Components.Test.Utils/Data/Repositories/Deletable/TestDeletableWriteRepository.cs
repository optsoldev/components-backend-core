using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Deletable
{
    public class TestDeletableWriteRepository : Repository<TestDeletableEntity, Guid>, ITestDeletableWriteRepository
    {
        public TestDeletableWriteRepository(CoreContext context, ILogger<Repository<TestDeletableEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }


}
