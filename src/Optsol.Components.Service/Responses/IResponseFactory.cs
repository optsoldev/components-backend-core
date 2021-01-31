using Optsol.Components.Application.DataTransferObject;
using Optsol.Components.Application.Result;

namespace Optsol.Components.Service.Responses
{
    public interface IResponseFactory
    {
        Response Create(ServiceResult serviceResult);

        Response<TData> Create<TData>(ServiceResult<TData> serviceResult) 
            where TData : BaseDataTransferObject;

        ResponseList<TData> Create<TData>(ServiceResultList<TData> serviceResult) 
            where TData : BaseDataTransferObject;
    }
}
