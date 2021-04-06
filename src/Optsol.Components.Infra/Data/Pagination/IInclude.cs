using System.Linq;
using System;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data.Pagination
{
    public interface IInclude<TEntity> 
        where TEntity: IEntity
    {
        Func<IQueryable<TEntity>, IQueryable<TEntity>> GetInclude();
    }
}
