using FluentValidation;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class EmailValueObjectContract : AbstractValidator<EmailValueObject>
    {
        public EmailValueObjectContract()
        {
            RuleFor(entity => entity.Email).EmailAddress().WithMessage("E-mail inválido");
        }
    }
}
