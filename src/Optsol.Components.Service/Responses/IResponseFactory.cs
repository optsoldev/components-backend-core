using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Infra.Data;
using System.Collections.Generic;

namespace Optsol.Components.Service.Responses
{
    public interface IResponseFactory
    {
        Response Create();

        Response<TData> Create<TData>(TData serviceResult)
            where TData : BaseDataTransferObject;

        ResponseList<TData> Create<TData>(IEnumerable<TData> serviceResult)
            where TData : BaseDataTransferObject;

        ResponseSearch<TData> Create<TData>(SearchResult<TData> searchResult)
            where TData : BaseDataTransferObject;
    }
}
