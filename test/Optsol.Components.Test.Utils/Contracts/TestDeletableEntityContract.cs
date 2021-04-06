using Flunt.Validations;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestDeletableEntityContract : Contract<TestDeletableEntity>
    {
        public TestDeletableEntityContract(TestDeletableEntity testDeletableEntity)
        {
            Requires()
                .IsNotNull(testDeletableEntity.Nome, "Nome", "O Nome não pode ser nulo")
                .IsNotNull(testDeletableEntity.Email, "Email", "O Email não pode ser nulo");
        }
    }
}
