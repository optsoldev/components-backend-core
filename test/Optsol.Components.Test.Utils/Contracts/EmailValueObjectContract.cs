using Flunt.Validations;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class EmailValueObjectContract : Contract<EmailValueObject>
    {
        public EmailValueObjectContract(EmailValueObject emailValueObject)
        {
            Requires()
               .IsEmail(emailValueObject.Email, "Email", "E-mail inválido");
        }
    }
}
