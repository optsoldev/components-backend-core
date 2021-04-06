using Optsol.Components.Domain.Pagination;
using System.Collections.Generic;

namespace Optsol.Components.Infra.Data
{
    public class SearchResult<TData> : ISearchResult<TData> where TData : class
    {
        private uint page;

        public uint Page
        {
            get
            {
                return page;
            }

            set
            {
                page = value <= 0 ? 1 : value;
            }
        }

        public uint? PageSize { get; set; }

        public long Total { get; set; }

        public long TotalItems { get; set; }

        public IEnumerable<TData> Items { get; set; }

        public SearchResult(uint page, uint? pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }
    }
}
