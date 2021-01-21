using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;

namespace Optsol.Components.Test.Utils.Data
{
    public class TestWriteRepository : Repository<TestEntity, Guid>, ITestWriteRepository
    {
        public TestWriteRepository(DbContext context, ILogger<Repository<TestEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }


}
