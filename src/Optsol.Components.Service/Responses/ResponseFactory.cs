using System;
using System.Collections.Generic;
using System.Linq;
using Flunt.Notifications;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Application.Results;

namespace Optsol.Components.Service.Responses
{
    public class ResponseFactory : IResponseFactory
    {
        public Response Create(ServiceResult serviceResult)
        {
            return new Response(serviceResult.Valid, MessageResolver(serviceResult.Notifications));
        }

        public Response<TData> Create<TData>(ServiceResult<TData> serviceResult)
            where TData : BaseDataTransferObject
        {
            return new Response<TData>(serviceResult.Data, serviceResult.Valid, MessageResolver(serviceResult.Notifications));
        }

        public ResponseList<TData> Create<TData>(ServiceResultList<TData> serviceResult) where TData : BaseDataTransferObject
        {
            return new ResponseList<TData>(serviceResult.DataList, serviceResult.Valid, MessageResolver(serviceResult.Notifications));
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
