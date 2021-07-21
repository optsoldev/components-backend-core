using System;
using Optsol.Components.Domain.Entities;
using Optsol.Playground.Domain.Validators;

namespace Optsol.Playground.Domain.Entities
{
    public class CartaoCreditoEntity : Entity<Guid>
    {
        public string NomeCliente { get; private set; }

        public string Numero { get; private set; }

        public string CodigoVerificacao { get; private set; }

        public DateTime Validade { get; private set; }

        public bool Valido
        {
            get
            {
                return ObterSituacaoValidade();
            }
        }

        public Guid ClienteId { get; private set; }

        public ClienteEntity Cliente { get; private set; }

        public CartaoCreditoEntity()
        {
        }

        public CartaoCreditoEntity(Guid id, string nomeCliente, string numero, string codigoVerificacao, DateTime validade, Guid clienteId)
            : this(nomeCliente, numero, codigoVerificacao, validade, clienteId)
        {
            Id = id;
        }

        public CartaoCreditoEntity(string nomeCliente, string numero, string codigoVerificacao, DateTime validade, Guid clienteId)
        {
            NomeCliente = nomeCliente;
            Numero = numero;
            CodigoVerificacao = codigoVerificacao;
            Validade = validade;
            ClienteId = clienteId;
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            Validate();
        }

        public override void Validate()
        {
            var validation = new CartaoCreditoEntityContract();
            var resultOfValidation = validation.Validate(this);

            AddNotifications(resultOfValidation);

            base.Validate();
        }

        private bool ObterSituacaoValidade()
        {
            return DateTime.Now.Subtract(Validade).TotalDays < 0;
        }
    }
}
