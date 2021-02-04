using Flunt.Validations;
using Optsol.Components.Domain.Entities;
using System;

namespace Optsol.Components.Test.Utils.Data
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
            base.Validate();

            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(Nome, "Nome", "O Nome não pode ser nulo")
                .IsNotNull(Email, "Email", "O Email não pode ser nulo"));

            if (Invalid)
                return;

            AddNotifications(Nome, Email);

        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedDate = DateTime.Now;
        }
    }
}
