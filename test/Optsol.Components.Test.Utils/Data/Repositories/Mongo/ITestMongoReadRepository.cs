using Optsol.Components.Test.Utils.Entity.Entities;
using System;
using Optsol.Components.Domain.Repositories;

namespace Optsol.Components.Test.Utils.Data.Repositories.Mongo;
public interface ITestMongoReadRepository : IReadBaseRepository<TestEntity, Guid>
{
}