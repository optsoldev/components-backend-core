using System.Linq;
using Optsol.Components.Application.DataTransferObject;
using Optsol.Components.Application.Result;

namespace Optsol.Components.Service.Response
{
    public class ResponseFactory : IResponseFactory
    {
         public Response Create(ServiceResult serviceResult)
        {
            return new Response(serviceResult.Valid, serviceResult.Notifications.Select(s => s.Message));
        }

        public Response<TData> Create<TData>(ServiceResult<TData> serviceResult) 
            where TData : BaseDataTransferObject
        {
            return new Response<TData>(serviceResult.Data, serviceResult.Valid, serviceResult.Notifications.Select(s => s.Message));
        }

        public ResponseList<TData> Create<TData>(ServiceResultList<TData> serviceResult) where TData : BaseDataTransferObject
        {
            return new ResponseList<TData>(serviceResult.DataList, serviceResult.Valid, serviceResult.Notifications.Select(s => s.Message));
        }
    }
}
