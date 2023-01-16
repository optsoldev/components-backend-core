using FluentValidation;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Domain.Clientes.Validators;

public class NomeValueObjectContract : AbstractValidator<NomeValueObject>
{
    public NomeValueObjectContract()
    {
        RuleFor(entity => entity.Nome)
            .MinimumLength(3)
            .MaximumLength(70)
            .WithMessage("O nome deve conter de 3 a 70 caracteres");

        RuleFor(entity => entity.Nome)
            .MinimumLength(3)
            .MaximumLength(70)
            .WithMessage("O nome deve conter de 3 a 70 caracteres");
    }
}