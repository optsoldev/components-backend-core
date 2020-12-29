using System.Linq;
using System;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data
{
    public interface IOrderBy<TEntity, TKey> 
        where TEntity: IAggregateRoot<TKey>
    {
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy();
    }
}
