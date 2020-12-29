using System.Linq;
using System;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data
{
    public interface IInclude<TEntity, TKey> 
        where TEntity: IAggregateRoot<TKey>
    {
        Func<IQueryable<TEntity>, IQueryable<TEntity>> GetInclude();
    }
}
