using Optsol.Components.Domain;

namespace Optsol.Components.Test.Shared.Data
{
    public class TestEntity: AggregateRoot
    {
        public NomeValueObject Nome { get; private set; }
        public EmailValueObject Email { get; private set; }
        public bool Ativo { get; private set; }

        public TestEntity()
        {
        }
        
        public TestEntity(NomeValueObject nome, EmailValueObject email)
        {
            Nome = nome;
            Email = email;

            AddNotifications(nome, email);

            Ativo = false;
        }
    }
}
