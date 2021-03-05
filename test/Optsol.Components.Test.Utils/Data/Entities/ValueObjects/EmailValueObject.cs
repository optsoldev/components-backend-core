using Flunt.Validations;
using Optsol.Components.Domain.ValueObjects;

namespace Optsol.Components.Test.Utils.Data.Entities.ValueObjecs
{
    public class EmailValueObject : ValueObject
    {
        public string Email { get; private set; }

        public EmailValueObject(string email)
        {
            Email = email;

            Validate();
        }

        public override string ToString()
        {
            return Email;
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
               .Requires()
               .IsEmail(Email, $"{nameof(EmailValueObject.Email)}", "E-mail inválido")
           );
        }
    }
}
