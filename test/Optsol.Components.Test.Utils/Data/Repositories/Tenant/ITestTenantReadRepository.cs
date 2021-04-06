using Optsol.Components.Domain.Data;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Repositories.Tenant
{
    public interface ITestTenantReadRepository: IReadRepository<TestTenantEntity, Guid>
    {
    }

    public interface ITestTenantDeletableReadRepository : IReadRepository<TestTenantDeletableEntity, Guid>
    {
    }
}
