using FluentValidation;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestTenantEntityContract : AbstractValidator<TestTenantEntity>
    {
        public TestTenantEntityContract()
        {
            RuleFor(entity => entity.Nome).SetValidator(new NomeValueObjectContract());
            RuleFor(entity => entity.Email).SetValidator(new EmailValueObjectContract());
            RuleFor(entity => entity.TenantId).NotEmpty().WithMessage("O tenant id deve ser informado");
        }
    }
}
