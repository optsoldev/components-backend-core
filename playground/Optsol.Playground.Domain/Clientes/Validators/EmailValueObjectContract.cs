using FluentValidation;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Domain.Clientes.Validators;

public class EmailValueObjectContract : AbstractValidator<EmailValueObject>
{
    public EmailValueObjectContract()
    {
        RuleFor(entity => entity.Email)
            .EmailAddress()
            .WithMessage("E-mail inválido");
    }
}