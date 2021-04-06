using Optsol.Components.Domain.Data;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Core
{
    public interface ITestWriteRepository: IWriteRepository<TestEntity, Guid>
    {
    }
}
