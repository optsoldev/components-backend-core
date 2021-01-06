using System.Linq.Expressions;
using System;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data
{
    public interface ISearch<TEntity>
    {
        Expression<Func<TEntity,bool>> GetSearcher();
    }
}
