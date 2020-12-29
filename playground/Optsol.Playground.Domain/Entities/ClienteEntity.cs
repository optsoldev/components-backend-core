using System.Linq;
using System.Runtime.Versioning;
using System;
using System.Collections.Generic;
using Flunt.Validations;
using Optsol.Components.Domain.Entities;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Domain.Entities
{
    public class ClienteEntity : AggregateRoot
    {
        public NomeValueObject Nome { get; private set; }
        public EmailValueObject Email { get; private set; }

        public bool Ativo { get; private set; }
        public bool PossuiCartao 
        { 
            get 
            { 
                return ExisteCartoesValidos();
            }
        }

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

        public override void Validate()
        {
            base.Validate();

            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(Nome, "Nome", "O Nome não pode ser nulo")
                .IsNotNull(Email, "Email", "O Email não pode ser nulo"));

            AddNotifications(Nome, Email);
        }

        public ClienteEntity AdicionarCartao(CartaoCreditoEntity cartaoCreditoEntity)
        {
            cartaoCreditoEntity.Validate();
            if(cartaoCreditoEntity.Valid)
                this.Cartoes.Add(cartaoCreditoEntity);

            AddNotifications(cartaoCreditoEntity);

            return this;
        }

        private bool ExisteCartoesValidos()
        {
            return Cartoes.Any(a => a.Valido);
        }
    }
}
