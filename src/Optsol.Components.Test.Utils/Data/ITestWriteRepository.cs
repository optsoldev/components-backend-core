using System;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Shared.Data;

namespace Optsol.Components.Test.Utils.Data
{
    public interface ITestWriteRepository: IWriteRepository<TestEntity, Guid>
    {
    }
}
