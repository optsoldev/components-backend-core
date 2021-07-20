using Optsol.Components.Domain.ValueObjects;
using Optsol.Components.Test.Utils.Contracts;

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
            //TODO: REVER
            //AddNotifications(new EmailValueObjectContract(this));
        }
    }
}
