using System;
using Optsol.Playground.Domain.Clientes;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Domain.Entities;

public class ClientePessoaFisica : Cliente
{
    public string Documento { get; private set; }

    public ClientePessoaFisica()
    {

    }

    public ClientePessoaFisica(Guid id, NomeValueObject nome, EmailValueObject email, string documento)
        : base(id, nome, email)
    {
        Documento = documento;
    }

    public ClientePessoaFisica(NomeValueObject nome, EmailValueObject email, string documento)
        : base(nome, email)
    {
        Documento = documento;
    }
}