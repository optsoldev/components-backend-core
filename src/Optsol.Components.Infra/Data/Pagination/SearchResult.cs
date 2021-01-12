using System.Collections.Generic;

namespace Optsol.Components.Infra.Data
{
    public class SearchResult<TEntity>
        where TEntity : class
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
        public long TotalItens { get; set; }
        public IEnumerable<TEntity> Itens { get; set; }
        public SearchResult(uint page, uint? pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }
    }
}
