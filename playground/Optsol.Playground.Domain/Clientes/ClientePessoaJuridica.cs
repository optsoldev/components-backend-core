using Optsol.Playground.Domain.ValueObjects;
using System;
using Optsol.Playground.Domain.Clientes;

namespace Optsol.Playground.Domain.Entities
{
    public class ClientePessoaJuridica : Cliente
    {
        public string NumeroCnpj { get; private set; }

        public ClientePessoaJuridica()
        {

        }

        public ClientePessoaJuridica(Guid id, NomeValueObject nome, EmailValueObject email, string numeroCnpj)
            : base(id, nome, email)
        {
            NumeroCnpj = numeroCnpj;
        }

        public ClientePessoaJuridica(NomeValueObject nome, EmailValueObject email, string numeroCnpj)
            : base(nome, email)
        {
            NumeroCnpj = numeroCnpj;
        }
    }
}
