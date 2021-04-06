using Flunt.Validations;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestTenantDeletableEntityContract : Contract<TestTenantDeletableEntity>
    {
        public TestTenantDeletableEntityContract(TestTenantDeletableEntity testTenantDeletableEntity)
        {
            Requires()
                .IsNotNull(testTenantDeletableEntity.Nome, "Nome", "O Nome não pode ser nulo")
                .IsNotNull(testTenantDeletableEntity.Email, "Email", "O Email não pode ser nulo")
                .IsEmpty(testTenantDeletableEntity.TenantId, "TenantId", "O Tenant Id não pode estar vazio");
        }
    }
}
