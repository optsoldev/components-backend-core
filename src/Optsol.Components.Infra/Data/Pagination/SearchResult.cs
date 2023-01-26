using Optsol.Components.Domain.Pagination;
using System.Collections.Generic;

namespace Optsol.Components.Infra.Data.Pagination
{
    public class SearchResult<TData> : ISearchResult<TData> where TData : class
    {
        private uint page;

        public uint Page
        {
            get => page;

            set => page = value <= 0 ? 1 : value;
        }

        public uint? PageSize { get; set; }

        public long Total { get; set; }

        public long TotalItems { get; set; }

        public IEnumerable<TData> Items { get; set; }

        public ISearchResult<TData> SetPaginatedItems(IEnumerable<TData> itens)
        {
            Items = itens;

            return this;
        }

        public ISearchResult<TData> SetTotalCount(int total)
        {
            Total = total;

            return this;
        }
        
        public SearchResult(uint page, uint? pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
