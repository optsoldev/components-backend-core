using System.Collections.Generic;

namespace Optsol.Components.Infra.Data
{
    public class SearchResult<TEntity>
        where TEntity : class
    {
        public int Page { get; set; }
        public int? PageSize { get; set; }
        public long Total { get; set; }
        public long TotalItens { get; set; }
        public IEnumerable<TEntity> Itens { get; set; }
        public SearchResult(int page, int? pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }
    }
}
