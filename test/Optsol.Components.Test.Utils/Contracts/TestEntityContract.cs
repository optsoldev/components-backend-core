using Flunt.Validations;
using Optsol.Components.Test.Utils.Entity.Entities;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestEntityContract : Contract<TestEntity>
    {
        public TestEntityContract(TestEntity testEntity)
        {
            Requires()
                .IsNotNull(testEntity.Nome, "Nome", "O Nome não pode ser nulo")
                .IsNotNull(testEntity.Email, "Email", "O Email não pode ser nulo");
        }
    }
}
