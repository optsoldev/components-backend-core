using System;
using Optsol.Components.Infra.Data;

namespace Optsol.Components.Test.Utils.Data
{
    public interface ITestDeletableReadRepository: IReadRepository<TestDeletableEntity, Guid>
    {
    }
}
