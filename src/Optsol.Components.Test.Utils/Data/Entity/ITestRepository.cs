using System;
using Optsol.Components.Infra.Data;

namespace Optsol.Components.Test.Utils.Entity
{
    public interface ITestReadRepository: IReadRepository<TestEntity, Guid>
    {
    }
}
