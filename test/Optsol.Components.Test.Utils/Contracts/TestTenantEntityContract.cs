using Flunt.Validations;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestTenantEntityContract : Contract<TestTenantEntity>
    {
        public TestTenantEntityContract(TestTenantEntity testTenantEntity)
        {
            Requires()
                .IsNotNull(testTenantEntity.Nome, "Nome", "O Nome não pode ser nulo")
                .IsNotNull(testTenantEntity.Email, "Email", "O Email não pode ser nulo")
                .IsFalse(testTenantEntity.TenantId == Guid.Empty, "TenantId", "O Tenant Id não pode estar vazio");
        }
    }
}
