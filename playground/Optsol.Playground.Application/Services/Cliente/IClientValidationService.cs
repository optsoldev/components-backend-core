using System.Threading.Tasks;
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

    public override Task InsertValidationAsync()
    {
        return Task.CompletedTask;
    }

    public override Task UpdateValidationAsync()
    {
        return Task.CompletedTask;
    }
}