using System;

namespace Optsol.Components.Infra.Data
{
    public class RequestSearch<TSearch>
        where TSearch: class  
    {
        public TSearch Search { get; set; }

        public uint page;
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
    }
}
