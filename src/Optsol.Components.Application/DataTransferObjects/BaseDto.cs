using FluentValidation.Results;
using Optsol.Components.Shared.Notifications;

namespace Optsol.Components.Application.DataTransferObjects
{
    public abstract class BaseDto : Notifiable<Notification>
    {
        public virtual void Validate() { }

        public void AddNotifications(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    AddNotification(failure.PropertyName, failure.ErrorMessage);
                }
            }
        }
    }
}
