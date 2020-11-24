using System;
using Flunt.Validations;
using Optsol.Components.Domain.Entities;

namespace Optsol.Playground.Domain.Entidades
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
            CreateDate = DateTime.Now;
            Validate();
        }

        public override void Validate()
        {
            base.Validate();

            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(NomeCliente, "NomeCliente", "O Nome do cliente não pode ser nulo")
                .IsNotNullOrEmpty(Numero, "Numero", "O Numero não pode ser nulo")
                .IsNotNullOrEmpty(CodigoVerificacao, "CodigoVerificacao", "O Codigo Verificacao do cliente não pode ser nulo")
                .IsNotEmpty(ClienteId, "ClienteId", "O Nome do cliente não pode ser nulo")
            );
        }

        private bool ObterSituacaoValidade()
        {
            return DateTime.Now.Subtract(Validade).TotalDays < 0;
        }
    }
}
