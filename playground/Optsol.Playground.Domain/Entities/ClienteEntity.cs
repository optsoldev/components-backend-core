using Optsol.Components.Domain.Entities;
using Optsol.Playground.Domain.Validators;
using Optsol.Playground.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

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
            AddNotifications(new ClienteEntityContract(this));

            AddNotifications(Nome, Email);

            base.Validate();
        }

        public ClienteEntity AdicionarCartao(CartaoCreditoEntity cartaoCreditoEntity)
        {
            cartaoCreditoEntity.Validate();
            if (cartaoCreditoEntity.IsValid)
                Cartoes.Add(cartaoCreditoEntity);

            AddNotifications(cartaoCreditoEntity);

            return this;
        }

        private bool ExisteCartoesValidos()
        {
            return Cartoes.Any(a => a.Valido);
        }
    }

    public class ClientePessoaFisicaEntity : ClienteEntity
    {
        public string Documento { get; private set; }

        public ClientePessoaFisicaEntity()
        {

        }

        public ClientePessoaFisicaEntity(Guid id, NomeValueObject nome, EmailValueObject email, string documento) 
            : base(id, nome, email)
        {
            Documento = documento;
        }

        public ClientePessoaFisicaEntity(NomeValueObject nome, EmailValueObject email, string documento) 
            : base(nome, email)
        {
            Documento = documento;
        }
    }

    public class ClientePessoaJuridicaEntity : ClienteEntity
    {
        public string NumeroCnpj { get; private set; }

        public ClientePessoaJuridicaEntity()
        {

        }

        public ClientePessoaJuridicaEntity(Guid id, NomeValueObject nome, EmailValueObject email, string numeroCnpj) 
            : base(id, nome, email)
        {
            NumeroCnpj = numeroCnpj;
        }

        public ClientePessoaJuridicaEntity(NomeValueObject nome, EmailValueObject email, string numeroCnpj) 
            : base(nome, email)
        {
            NumeroCnpj = numeroCnpj;
        }
    }
}
