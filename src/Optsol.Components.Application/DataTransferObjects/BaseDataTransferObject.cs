using FluentValidation.Results;
using Optsol.Components.Shared.Notifications;

namespace Optsol.Components.Application.DataTransferObjects
{
    public abstract class BaseDataTransferObject : Notifiable<Notification>
    {
        public abstract void Validate();

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
