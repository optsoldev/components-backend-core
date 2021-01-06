using System.Linq;
using System;

namespace Optsol.Components.Infra.Data
{
    public interface IOrderBy<TEntity> 
    {
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy();
    }
}
