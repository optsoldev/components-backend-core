using System;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Repositories.Tenant
{
    public interface ITestTenantReadRepository: IReadRepository<TestTenantEntity, Guid>
    {
    }
}
