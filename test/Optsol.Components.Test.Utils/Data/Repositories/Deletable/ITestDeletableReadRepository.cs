using System;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Repositories.Deletable
{
    public interface ITestDeletableReadRepository: IReadRepository<TestDeletableEntity, Guid>
    {
    }
}
