using System;
using Optsol.Components.Domain.Repositories;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Data.Repositories.Tenants
{
    public interface ITestTenantReadRepository: IReadRepository<TestTenantEntity, Guid>
    {
    }

    public interface ITestTenantDeletableReadRepository : IReadRepository<TestTenantDeletableEntity, Guid>
    {
    }
}
