using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Notifications;

namespace Optsol.Playground.Application.Services.Cliente;

public interface IClientValidationService : IValidationService
{
    
}

public class ClientValidationService : BaseValidationService, IClientValidationService
{
    public ClientValidationService(NotificationContext notificationContext) : base(notificationContext)
    {
    }

    public override void InsertValidation()
    {
    }

    public override void UpdateValidation()
    {
    }
}