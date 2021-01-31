using Flunt.Notifications;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Application.Results;
using Optsol.Components.Domain.Notifications;
using System;
using System.Collections.Generic;

namespace Optsol.Components.Service.Responses
{
    public class ResponseFactory : IResponseFactory
    {
        private readonly NotificationContext _notificationContext;

        public ResponseFactory(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public Response Create(ServiceResult serviceResult)
        {
            var responseSuccess = !_notificationContext.HasNotifications;

            return new Response(responseSuccess, MessageResolver(_notificationContext.Notifications));
        }

        public Response<TData> Create<TData>(ServiceResult<TData> serviceResult)
            where TData : BaseDataTransferObject
        {
            return new Response<TData>(serviceResult.Data, ResponseSuccess(), MessageResolver(_notificationContext.Notifications));
        }

        public ResponseList<TData> Create<TData>(ServiceResultList<TData> serviceResult) where TData : BaseDataTransferObject
        {
            var responseSuccess = !_notificationContext.HasNotifications;

            return new ResponseList<TData>(serviceResult.Data, ResponseSuccess(), MessageResolver(_notificationContext.Notifications));
        }

        private bool ResponseSuccess()
        {
            return !_notificationContext.HasNotifications;
        }

        readonly Func<IReadOnlyCollection<Notification>, List<string>> MessageResolver = (notifications) =>
        {
            var messages = new List<string>();

            foreach (var notify in notifications)
                messages.Add($"{notify.Property}:{notify.Message}");

            return messages;
        };
    }
}
