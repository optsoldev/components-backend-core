using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Entity;
using System;

namespace Optsol.Components.Test.Utils.Data.Mongo
{
    public interface ITestMongoReadRepository : IReadRepository<TestEntity, Guid>
    {
    }
}
