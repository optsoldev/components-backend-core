using System;
using Optsol.Components.Domain.Entities;
using Optsol.Playground.Domain.Clientes;
using Optsol.Playground.Domain.Clientes.Validators;

namespace Optsol.Playground.Domain.Entities
{
    public class CartaoCredito : Entity<Guid>
    {
        public string NomeCliente { get; private set; }

        public string Numero { get; private set; }

        public string CodigoVerificacao { get; private set; }

        public DateTime Validade { get; private set; }

        public bool Valido => ObterSituacaoValidade();

        public Guid ClienteId { get; private set; }

        public Cliente Cliente { get; private set; }

        public CartaoCredito()
        {
        }

        public CartaoCredito(Guid id, string nomeCliente, string numero, string codigoVerificacao, DateTime validade, Guid clienteId)
            : this(nomeCliente, numero, codigoVerificacao, validade, clienteId)
        {
            Id = id;
        }

        public CartaoCredito(string nomeCliente, string numero, string codigoVerificacao, DateTime validade, Guid clienteId)
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

        public sealed override void Validate()
        {
            var validation = new CartaoCreditoEntityContract();
            var resultOfValidation = validation.Validate(this);

            AddNotifications(resultOfValidation);

            base.Validate();
        }

        private bool ObterSituacaoValidade() => DateTime.Now.Subtract(Validade).TotalDays < 0;
        
    }
}
