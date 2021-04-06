using Flunt.Notifications;

namespace Optsol.Components.Application.DataTransferObjects
{
    public abstract class BaseDataTransferObject : Notifiable<Notification>
    {
        public abstract void Validate();
    }
}
