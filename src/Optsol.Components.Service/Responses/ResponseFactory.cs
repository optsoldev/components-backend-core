using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Domain.Pagination;
using Optsol.Components.Shared.Notifications;
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

        public Response Create()
        {
            return new Response(ResponseSuccess(), MessageResolver(_notificationContext.Notifications));
        }

        public Response<TData> Create<TData>(TData data)
            where TData : BaseDto
        {
            return new Response<TData>(data, ResponseSuccess(), MessageResolver(_notificationContext.Notifications));
        }

        public ResponseList<TData> Create<TData>(IEnumerable<TData> dataList) where TData : BaseDto
        {
            return new ResponseList<TData>(dataList, ResponseSuccess(), MessageResolver(_notificationContext.Notifications));
        }

        public ResponseSearch<TData> Create<TData>(ISearchResult<TData> searchResult)
            where TData : BaseDto
        {
            return new ResponseSearch<TData>(searchResult, ResponseSuccess(), MessageResolver(_notificationContext.Notifications));
        }

        private bool ResponseSuccess()
        {
            return !_notificationContext.HasNotifications;
        }

        private static List<string> MessageResolver(IReadOnlyCollection<Notification> notifications)
        {
            var messages = new List<string>();

            foreach (var notify in notifications)
            {
                messages.Add($"{notify.Key}:{notify.Message}");
            }

            return messages;
        }
    }
}
