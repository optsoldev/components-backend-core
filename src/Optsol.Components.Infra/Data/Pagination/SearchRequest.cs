using Optsol.Components.Domain.Pagination;

namespace Optsol.Components.Infra.Data.Pagination
{
    public class SearchRequest<TSearch> : ISearchRequest<TSearch> where TSearch : class
    {
        private uint _page;

        public uint Page
        {
            get
            {
                return _page;
            }

            set
            {
                _page = SetPageValue(value);
            }
        }

        public TSearch Search { get; set; }

        public uint? PageSize { get; set; }

        public uint SetPageValue(uint value)
        {
            var isZero = value == 0;
            if (isZero)
            {
                return 1;
            }
            return value;
        }
    }
}
