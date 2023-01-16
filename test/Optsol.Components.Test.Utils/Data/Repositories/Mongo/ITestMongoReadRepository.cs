using Optsol.Components.Domain.Data;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;

namespace Optsol.Components.Test.Utils.Data.Repositories.Mongo;
public interface ITestMongoReadRepository : IReadBaseRepository<TestEntity, Guid>
{
}