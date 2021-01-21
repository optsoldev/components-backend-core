using System;
using Optsol.Components.Infra.Data;

namespace Optsol.Components.Test.Utils.Data
{
    public interface ITestDeletableWriteRepository : IWriteRepository<TestDeletableEntity, Guid>
    {
    }
}
