using System;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Entity.Entities;

namespace Optsol.Components.Test.Utils.Repositories.Core
{
    public interface ITestWriteRepository: IWriteRepository<TestEntity, Guid>
    {
    }
}
