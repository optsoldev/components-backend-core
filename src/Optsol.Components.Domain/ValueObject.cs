using Flunt.Notifications;

namespace Optsol.Components.Domain
{
    public abstract class ValueObject: Notifiable
    {
        public abstract void Validate();
    }
}
