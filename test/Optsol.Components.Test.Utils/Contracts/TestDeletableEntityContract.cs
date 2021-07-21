using FluentValidation;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestDeletableEntityContract : AbstractValidator<TestDeletableEntity>
    {
        public TestDeletableEntityContract()
        {
            RuleFor(entity => entity.Nome).NotNull().WithMessage("Não pode ser nulo");

            RuleFor(entity => entity.Email).SetValidator(new EmailValueObjectContract());
        }
    }
}
