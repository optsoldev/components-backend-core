using Flunt.Notifications;
using Flunt.Validations;

namespace Optsol.Components.Application.DataTransferObjects
{
    public abstract class BaseDataTransferObject : Notifiable, IValidatable
    {
        public abstract void Validate();
    }
}
