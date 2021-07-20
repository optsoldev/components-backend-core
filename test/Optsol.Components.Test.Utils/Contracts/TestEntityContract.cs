using FluentValidation;
using Optsol.Components.Test.Utils.Entity.Entities;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestEntityContract : AbstractValidator<TestEntity>
    {
        public TestEntityContract()
        {
            RuleFor(entity => entity.Nome).SetValidator(new NomeValueObjectContract());
            RuleFor(entity => entity.Email).SetValidator(new EmailValueObjectContract());
        }
    }
}
