using System;

namespace Optsol.Components.Infra.Data
{
    public interface IRequestSearch<TSearch>
        where TSearch: class  
    {
        TSearch Search { get; set; }
        int Page { get; set; }
        int? PageSize { get; set; }
    }
}
