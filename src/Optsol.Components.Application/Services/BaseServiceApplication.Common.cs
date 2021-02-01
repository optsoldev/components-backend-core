using Microsoft.Extensions.Logging;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Shared.Extensions;

namespace Optsol.Components.Application.Services
{
    public partial class BaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData> 
        : IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>
        where TEntity : AggregateRoot
        where TGetByIdDto : BaseDataTransferObject
        where TGetAllDto : BaseDataTransferObject
        where TInsertData : BaseDataTransferObject
        where TUpdateData : BaseDataTransferObject
    {
        private void LogNotifications(string method)
        {
            if (_notificationContext.HasNotifications)
                _logger?.LogInformation($"Método: { method } Invalid: { _notificationContext.HasNotifications } Notifications: { _notificationContext.Notifications.ToJson() }");
        }
    }
}
