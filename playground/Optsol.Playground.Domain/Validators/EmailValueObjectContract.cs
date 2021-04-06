using Flunt.Validations;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Domain.Validators
{
    public class EmailValueObjectContract : Contract<EmailValueObject>
    {
        public EmailValueObjectContract(EmailValueObject emailValueObject)
        {
            Requires()
               .IsEmail(emailValueObject.Email, $"{nameof(EmailValueObject.Email)}", "E-mail inválido");
        }
    }
}
