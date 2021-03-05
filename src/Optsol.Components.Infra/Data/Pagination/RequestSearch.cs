using System;

namespace Optsol.Components.Infra.Data
{
    public class SearchRequest<TSearch>
        where TSearch : class
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

        readonly Func<uint, uint> SetPageValue = 
            (value) =>
            {
                var isZero = value == 0;
                if (isZero)
                {
                    return 1;
                }
                return value;
            };
    }
}
