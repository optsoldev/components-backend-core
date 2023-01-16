using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Optsol.Components.Domain.Entities;
using Optsol.Playground.Domain.Clientes.Validators;
using Optsol.Playground.Domain.Entities;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Domain.Clientes;

public class Cliente : AggregateRoot, ITenant<Guid>
{
    private IList<CartaoCredito> cartoes => new List<CartaoCredito>();
    public IReadOnlyCollection<CartaoCredito> Cartoes => new ReadOnlyCollection<CartaoCredito>(cartoes);
    public Guid TenantId { get; private set; }
    public NomeValueObject Nome { get; private set; }
    public EmailValueObject Email { get; private set; }
    public bool Ativo { get; private set; }
    public bool PossuiCartao => ExisteCartoesValidos();

    public Cliente()
    {
    }

    public Cliente(Guid id, NomeValueObject nome, EmailValueObject email)
        : this(nome, email)
    {
        Id = id;
    }

    public Cliente(NomeValueObject nome, EmailValueObject email)
    {
        Nome = nome;
        Email = email;

        Validate();

        Ativo = false;
    }

    public sealed override void Validate()
    {
        var validation = new ClienteEntityContract();
        var resultOfValidation = validation.Validate(this);

        AddNotifications(resultOfValidation);

        base.Validate();
    }

    public Cliente AdicionarCartao(CartaoCredito cartaoCreditoEntity)
    {
        cartoes.Add(cartaoCreditoEntity);

        AddNotifications(cartaoCreditoEntity);

        return this;
    }

    private bool ExisteCartoesValidos()
    {
        return Cartoes.Any(a => a.Valido);
    }

    public void SetTenantId(Guid tenantId)
    {
        TenantId = tenantId;
    }
}