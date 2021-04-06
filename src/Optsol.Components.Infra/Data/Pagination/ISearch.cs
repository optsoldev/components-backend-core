using System.Linq.Expressions;
using System;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data.Pagination
{
    public interface ISearch<TEntity>
        where TEntity: IEntity
    {
        Expression<Func<TEntity,bool>> GetSearcher();
    }
}
