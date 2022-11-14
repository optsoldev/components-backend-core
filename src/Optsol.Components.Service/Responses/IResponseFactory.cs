using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Pagination;
using System.Collections.Generic;

namespace Optsol.Components.Service.Responses
{
    public interface IResponseFactory
    {
        Response Create();

        Response<TData> Create<TData>(TData data) where TData : BaseDto;

        ResponseList<TData> Create<TData>(IEnumerable<TData> dataList)
            where TData : BaseDto;

        ResponseSearch<TData> Create<TData>(ISearchResult<TData> searchResult)
            where TData : BaseDto;
    }
}
