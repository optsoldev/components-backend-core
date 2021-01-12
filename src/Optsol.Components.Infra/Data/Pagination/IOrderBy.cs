using System.Linq;
using System;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data
{
    public interface IOrderBy<TEntity> 
        where TEntity: IEntity
    {
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy();
    }
}
