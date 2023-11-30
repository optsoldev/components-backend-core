using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Notifications;
using System.Threading.Tasks;

namespace Optsol.Components.Application.Services;

/// <summary>
/// Interface created for validation 
/// </summary>
public interface IValidationService
{
    Task InsertValidationAsync();
    Task UpdateValidationAsync();
    void SetRequestModel<TRequest>(TRequest request) where TRequest : BaseModel;
    void SetEntity<TAggregateRoot>(TAggregateRoot entity) where TAggregateRoot : AggregateRoot;
}

public abstract class BaseValidationService
    : IValidationService
{
    protected BaseModel _requestModel;
    protected AggregateRoot _aggregateRoot;
    protected NotificationContext _notificationContext;
    
    protected BaseValidationService(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public abstract Task InsertValidationAsync();

    public abstract Task UpdateValidationAsync();

    public void SetRequestModel<TRequest>(TRequest request) where TRequest : BaseModel
    {
        _requestModel = request;
    }

    public void SetEntity<TAggregateRoot>(TAggregateRoot request) where TAggregateRoot : AggregateRoot
    {
        _aggregateRoot = request;
    }
}