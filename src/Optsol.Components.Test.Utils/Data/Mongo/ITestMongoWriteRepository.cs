using Optsol.Components.Infra.Data;
using System;

namespace Optsol.Components.Test.Utils.Entity.Test
{
    public interface ITestMongoWriteRepository : IMontoWriteRepository<TestEntity, Guid>
    {
    }
}
