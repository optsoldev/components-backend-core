using Optsol.Components.Domain.ValueObjects;

namespace Optsol.Playground.Domain.ValueObjects
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
            //AddNotifications(new EmailValueObjectContract(this)); TODO: REVER
        }
    }
}
