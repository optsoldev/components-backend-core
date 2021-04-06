using Optsol.Components.Domain.Data;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Deletable
{
    public interface ITestDeletableReadRepository: IReadRepository<TestDeletableEntity, Guid>
    {
    }
}
