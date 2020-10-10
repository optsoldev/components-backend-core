using System;
using Optsol.Sdk.Infra.Data;
using Optsol.Sdk.Test.Shared.Data;

namespace Optsol.Sdk.Test.Utils.Data
{
    public interface ITestWriteRepository: IWriteRepository<TestEntity, Guid>
    {
    }
}
