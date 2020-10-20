using System.Linq;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.ViewModel;

namespace Optsol.Components.Service.Response
{
    public class ResponseFactory : IResponseFactory
    {
         public Response Create(ServiceResult serviceResult)
        {
            return new Response(serviceResult.Valid, serviceResult.Notifications.Select(s => s.Message));
        }

        public Response<TViewModel> Create<TViewModel>(ServiceResult<TViewModel> serviceResult) 
            where TViewModel : BaseViewModel
        {
            return new Response<TViewModel>(serviceResult.Data, serviceResult.Valid, serviceResult.Notifications.Select(s => s.Message));
        }

        public ResponseList<TViewModel> Create<TViewModel>(ServiceResultList<TViewModel> serviceResult) where TViewModel : BaseViewModel
        {
            return new ResponseList<TViewModel>(serviceResult.DataList, serviceResult.Valid, serviceResult.Notifications.Select(s => s.Message));
        }
    }
}
