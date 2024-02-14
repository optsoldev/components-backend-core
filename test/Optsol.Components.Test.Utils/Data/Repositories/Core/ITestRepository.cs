using Optsol.Components.Test.Utils.Entity.Entities;
using System;
using Optsol.Components.Domain.Repositories;

namespace Optsol.Components.Test.Utils.Repositories.Core
{
    public interface ITestReadRepository: IReadRepository<TestEntity, Guid>
    {
    }
}
