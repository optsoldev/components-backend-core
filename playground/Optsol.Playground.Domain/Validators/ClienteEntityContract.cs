using FluentValidation;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Domain.Validators
{
    public class ClienteEntityContract : AbstractValidator<ClienteEntity>
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
}
