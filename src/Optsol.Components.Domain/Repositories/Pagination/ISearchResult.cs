using System.Collections.Generic;

namespace Optsol.Components.Domain.Pagination
{
    public interface ISearchResult<TData> where TData : class
    {
        IEnumerable<TData> Items { get; set; }

        uint Page { get; set; }

        uint? PageSize { get; set; }

        long Total { get; set; }

        long TotalItems { get; set; }
    }
}