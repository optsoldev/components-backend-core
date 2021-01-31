using Flunt.Notifications;

namespace Optsol.Components.Application.DataTransferObjects
{
    public abstract class BaseDataTransferObject : Notifiable
    {
        public abstract void Validate();
    }
}
