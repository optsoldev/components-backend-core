using System;
using Flunt.Validations;
using Optsol.Components.Domain;

namespace Optsol.Playground.Domain.Entidades
{
    public class CartaoCreditoEntity : AggregateRoot
    {
        public string NomeCliente { get; private set; }
        public string Numero { get; private set; }
        public string CodigoVerificacao { get; private set; }
        public string Validade { get; private set; }
        public Guid ClienteId { get; private set; }
        public ClienteEntity Cliente { get; private set; }

        public CartaoCreditoEntity()
        {
        }

        public CartaoCreditoEntity(Guid id, string nomeCliente, string numero, string codigoVerificacao, string validade, Guid clienteId)
            : this(nomeCliente, numero, codigoVerificacao, validade, clienteId)
        {
            Id = Id;
        }

        public CartaoCreditoEntity(string nomeCliente, string numero, string codigoVerificacao, string validade, Guid clienteId)
        {
            NomeCliente = nomeCliente;
            Numero = numero;
            CodigoVerificacao = codigoVerificacao;
            Validade = validade;
            ClienteId = clienteId;

            Validate();
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(NomeCliente, "NomeCliente", "O Nome do cliente não pode ser nulo")
                .IsNotNullOrEmpty(Numero, "Numero", "O Numero não pode ser nulo")
                .IsNotNullOrEmpty(CodigoVerificacao, "CodigoVerificacao", "O Codigo Verificacao do cliente não pode ser nulo")
                .IsNotEmpty(ClienteId, "ClienteId", "O Nome do cliente não pode ser nulo")
            );
        }
    }
}
