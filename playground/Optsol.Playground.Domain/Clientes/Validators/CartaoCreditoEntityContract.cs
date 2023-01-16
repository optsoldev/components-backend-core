using FluentValidation;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Domain.Clientes.Validators;

public class CartaoCreditoEntityContract : AbstractValidator<CartaoCredito>
{
    public CartaoCreditoEntityContract()
    {
        RuleFor(entity => entity.NomeCliente)
            .NotNull()
            .NotEmpty()
            .WithMessage("O Nome do cliente não pode ser nulo");

        RuleFor(entity => entity.Numero).NotNull()
            .WithMessage("O Numero não pode ser nulo");

        RuleFor(entity => entity.CodigoVerificacao)
            .NotNull()
            .WithMessage("O Codigo Verificacao do cliente não pode ser nulo");
            
        RuleFor(entity => entity.ClienteId)
            .NotNull()
            .WithMessage("O Nome do cliente não pode ser nulo");
    }
}