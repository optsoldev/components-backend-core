using System;
using System.Collections.Generic;
using Flunt.Validations;
using Optsol.Components.Domain;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Domain.Entidades
{
    public class ClienteEntity : AggregateRoot
    {
        public NomeValueObject Nome { get; private set; }
        public EmailValueObject Email { get; private set; }
        public bool Ativo { get; private set; }
        public virtual ICollection<CartaoCreditoEntity> Cartoes { get; private set; } = new List<CartaoCreditoEntity>();

        public ClienteEntity()
        {
        }

        public ClienteEntity(Guid id, NomeValueObject nome, EmailValueObject email)
            : this(nome, email)
        {
            Id = id;
        }

        public ClienteEntity(NomeValueObject nome, EmailValueObject email)
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

    }
}
