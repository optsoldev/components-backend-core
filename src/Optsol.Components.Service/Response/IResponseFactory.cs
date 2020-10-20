using Optsol.Components.Application.Result;
using Optsol.Components.Application.ViewModel;

namespace Optsol.Components.Service.Response
{
    public interface IResponseFactory
    {
        Response Create(ServiceResult serviceResult);

        Response<TViewModel> Create<TViewModel>(ServiceResult<TViewModel> serviceResult) 
            where TViewModel : BaseViewModel;

        ResponseList<TViewModel> Create<TViewModel>(ServiceResultList<TViewModel> serviceResult) 
            where TViewModel : BaseViewModel;
    }
}
