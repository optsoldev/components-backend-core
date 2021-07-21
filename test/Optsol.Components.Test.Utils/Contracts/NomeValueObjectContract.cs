using FluentValidation;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class NomeValueObjectContract : AbstractValidator<NomeValueObject>
    {
        public NomeValueObjectContract()
        {
            RuleFor(entity => entity.Nome).MinimumLength(3).MaximumLength(70).WithMessage("O nome deve conter de 3 a 70 caracteres");

            RuleFor(entity => entity.Nome).MinimumLength(3).MaximumLength(70).WithMessage("O nome deve conter de 3 a 70 caracteres");
        }
    }
}
