using Flunt.Validations;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Bus.Events;
using Optsol.Components.Test.Utils.Contracts;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using System;

namespace Optsol.Components.Test.Utils.Entity.Entities
{
    public class TestEntity : AggregateRoot, IEvent
    {
        public NomeValueObject Nome { get; private set; }

        public EmailValueObject Email { get; private set; }
        
        public bool Ativo { get; private set; }

        public TestEntity()
        {
        }

        public TestEntity(Guid id, NomeValueObject nome, EmailValueObject email)
            : this(nome, email)
        {
            Id = id;
        }

        public TestEntity(NomeValueObject nome, EmailValueObject email)
        {
            Nome = nome;
            Email = email;

            AddNotifications(Nome, Email);

            Ativo = false;
        }

        public void InserirNome(NomeValueObject nomeValueObject)
        {
            Nome = nomeValueObject;
        }

        public override void Validate()
        {
            AddNotifications(new TestEntityContract(this));

            base.Validate();
        }
    }
}
