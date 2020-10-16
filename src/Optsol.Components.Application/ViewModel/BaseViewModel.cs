using Flunt.Notifications;

namespace Optsol.Components.Application.ViewModel
{
    public abstract class BaseViewModel : Notifiable
    {
        public abstract void Validate();
    }
}
