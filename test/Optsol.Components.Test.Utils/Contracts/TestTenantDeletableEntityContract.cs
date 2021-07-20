using FluentValidation;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestTenantDeletableEntityContract : AbstractValidator<TestTenantDeletableEntity>
    {
        public TestTenantDeletableEntityContract()
        {
            //TODO: REVER
            //Requires()
            //    .IsNotNull(testTenantDeletableEntity.Nome, "Nome", "O Nome não pode ser nulo")
            //    .IsNotNull(testTenantDeletableEntity.Email, "Email", "O Email não pode ser nulo")
            //    .IsEmpty(testTenantDeletableEntity.TenantId, "TenantId", "O Tenant Id não pode estar vazio");
        }
    }
}
