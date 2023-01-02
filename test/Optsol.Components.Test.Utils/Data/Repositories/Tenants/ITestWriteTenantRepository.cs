using System;
using Optsol.Components.Domain.Data;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Data.Repositories.Tenants
{
    public interface ITestTenantWriteRepository : IWriteRepository<TestTenantEntity, Guid>
    {

    }
}
