using System.Linq;
using System;

namespace Optsol.Components.Infra.Data
{
    public interface IInclude<TEntity> 
    {
        Func<IQueryable<TEntity>, IQueryable<TEntity>> GetInclude();
    }
}
