using FluentValidation;

namespace Optsol.Playground.Domain.Clientes.Validators;

public class ClienteEntityContract : AbstractValidator<Cliente>
{
    public ClienteEntityContract()
    {
        RuleFor(entity => entity.Nome)
            .NotNull()
            .WithMessage("Não pode ser nulo");

        RuleFor(entity => entity.Email)
            .NotNull()
            .WithMessage("Não pode ser nulo");
    }
}