using System.Collections.Generic;

namespace Optsol.Components.Domain.Pagination
{
    public interface ISearchResult<TData> where TData : class
    {
        IEnumerable<TData> Items { get; }

        uint Page { get; set; }

        uint? PageSize { get; }

        long Total { get; }

        long TotalItems { get; }

        ISearchResult<TData> SetPaginatedItems(IEnumerable<TData> data);

        ISearchResult<TData> SetTotalCount(int total);
    }
}