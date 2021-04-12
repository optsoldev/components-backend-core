using Optsol.Components.Domain.Entities;
using System;
using System.Linq;

namespace Optsol.Components.Infra.Data.Pagination
{
    public class Include<TEntity> : IInclude<TEntity>
        where TEntity : IEntity
    {
        private readonly Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes;

        public Include(Func<IQueryable<TEntity>, IQueryable<TEntity>> expression)
        {
            Includes = expression;
        }
                
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> GetInclude()
        {
            return this.Includes;
        }
    }
}
