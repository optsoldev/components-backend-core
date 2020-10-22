using Flunt.Notifications;

namespace Optsol.Components.Application.DataTransferObject
{
    public abstract class BaseDataTransferObject : Notifiable
    {
        public abstract void Validate();
    }
}
