using Microsoft.EntityFrameworkCore;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Shared.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Data
{
    public partial class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        private async Task<SearchResult<TEntity>> CreateSearchResult(IQueryable<TEntity> query, uint page, uint? pageSize)
        {
            var searchResult = new SearchResult<TEntity>(page, pageSize);

            searchResult.Total = await query.CountAsync();

            query = ApplyPagination(query, page, pageSize);

            searchResult.Items = await query.AsAsyncEnumerable().AsyncEnumerableToEnumerable();
            searchResult.TotalItems = searchResult.Items.Count();

            return searchResult;
        }

        private IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, uint page, uint? pageSize)
        {
            var skip = --page * (pageSize ?? 0);

            query = query.Skip(skip.ToInt());

            if (pageSize.HasValue)
            {
                query = query.Take(pageSize.Value.ToInt());
            }

            return query;
        }

        private IQueryable<TEntity> ApplySearch(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> search = null)
        {
            var searchIsNotNull = search != null;
            if (searchIsNotNull)
            {
                query = query.Where(search);
            }

            return query;
        }

        private IQueryable<TEntity> ApplyInclude(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var includesIsNotNull = includes != null;
            if (includesIsNotNull)
            {
                query = includes(query);
            }

            return query;
        }

        private IQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var orderByIsNotNull = orderBy != null;
            if (orderByIsNotNull)
            {
                query = orderBy(query);
            }

            return query;
        }
    }
}
