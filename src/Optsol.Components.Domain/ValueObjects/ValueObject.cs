using Flunt.Notifications;

namespace Optsol.Components.Domain.ValueObjects
{
    public abstract class ValueObject: Notifiable
    {
        public abstract void Validate();
    }
}
