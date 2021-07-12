using Optsol.Components.Domain.Entities;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using System;

namespace Optsol.Components.Test.Utils.Data.Entities
{
    public class TestDeletableEntity : AggregateRoot, IDeletable
    {
        public NomeValueObject Nome { get; private set; }

        public EmailValueObject Email { get; private set; }

        public bool Ativo { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTime? DeletedDate { get; private set; }

        public TestDeletableEntity()
        {
        }


        public TestDeletableEntity(Guid id, NomeValueObject nome, EmailValueObject email)
            : this(nome, email)
        {
            Id = id;
        }

        public TestDeletableEntity(NomeValueObject nome, EmailValueObject email)
        {
            Nome = nome;
            Email = email;

            Validate();

            Ativo = false;
        }

        public void InserirNome(NomeValueObject nomeValueObject)
        {
            Nome = nomeValueObject;
        }

        public override void Validate()
        {
            //TODO: REVER
            //AddNotifications(new TestDeletableEntityContract(this));

            //AddNotifications(Nome, Email);

            //base.Validate();

        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedDate = DateTime.Now;
        }
    }
}
