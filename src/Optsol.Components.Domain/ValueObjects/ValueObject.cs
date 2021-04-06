using Flunt.Notifications;

namespace Optsol.Components.Domain.ValueObjects
{
    public abstract class ValueObject: Notifiable<Notification>
    {
        public abstract void Validate();
    }
}
