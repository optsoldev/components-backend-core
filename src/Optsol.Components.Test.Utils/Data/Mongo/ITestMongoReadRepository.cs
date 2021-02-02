using Optsol.Components.Infra.Data;
using System;

namespace Optsol.Components.Test.Utils.Entity.Mongo
{
    public interface ITestMongoReadRepository : IBaseReadRepository<TestEntity, Guid>
    {
    }
}
