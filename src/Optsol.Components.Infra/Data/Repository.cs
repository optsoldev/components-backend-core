using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;

namespace Optsol.Components.Infra.Data
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>, IDisposable
        where TEntity : class, IAggregateRoot<TKey>
    {
        private ILogger _logger;

        public CoreContext Context { get; protected set; }

        public DbSet<TEntity> Set { get; protected set; }

        public Repository(CoreContext context, ILogger<Repository<TEntity, TKey>> logger)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Repository<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");

            Context = context ?? throw new DbContextNullException();
            this.Set = context.Set<TEntity>();
        }

        public virtual Task<TEntity> GetByIdAsync(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }( {{ id:{ id } }} ) Retorno: type { typeof(TEntity).Name }");

            return Set.FindAsync(id).AsTask();
        }

        public virtual Task<TEntity> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }( {{ id:{ id } }} ) Retorno: type { typeof(TEntity).Name }");

            return Includes(Set).Where(w => w.Id.Equals(id)).FirstAsync();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IAsyncEnumerable<{ typeof(TEntity).Name }>");

            return Set.AsAsyncEnumerable().AsyncEnumerableToEnumerable();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes)
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IAsyncEnumerable<{ typeof(TEntity).Name }>");

            return Includes(Set).AsAsyncEnumerable().AsyncEnumerableToEnumerable();
        }

        public virtual Task<SearchResult<TEntity>> GetAllAsync<TSearch>(SearchRequest<TSearch> requestSearch)
            where TSearch : class
        {
            var search = requestSearch.Search as ISearch<TEntity>;
            var orderBy = requestSearch.Search as IOrderBy<TEntity>;
            var include = requestSearch.Search as IInclude<TEntity>;

            IQueryable<TEntity> query = this.Set;

            query = ApplySearch(query, search?.GetSearcher());

            query = ApplyInclude(query, include?.GetInclude());

            query = ApplyOrderBy(query, orderBy?.GetOrderBy());

            return CreateSearchResult(query, requestSearch.Page, requestSearch.PageSize);
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(InsertAsync) }( {{entity:{ entity.ToJson() }}} )");

            return Set.AddAsync(entity).AsTask();
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }( {{entity:{ entity.ToJson() }}} )");

            var localEntity = Context.Set<TEntity>().Local?.Where(w => w.Id.Equals(entity.Id)).FirstOrDefault();
            var inLocal = localEntity != null;
            if (inLocal)
            {
                Context.Entry(localEntity).State = EntityState.Detached;
            }

            Set.Update(entity);

            return Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await Set.FindAsync(id);

            var entityNotFound = entity == null;
            if (entityNotFound)
            {
                _logger?.LogError($"Método: { nameof(DeleteAsync) }({{ TKey:{ id.ToJson() } }}) Registro não encontrado");
                return;
            }


            await DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }( {{entity:{ entity.ToJson() }}} )");

            var entityFound = entity != null;
            var entityIsDeletable = entity is IDeletable;

            if (entityFound && entityIsDeletable)
            {
                ((IDeletable)entity).Delete();
                await UpdateAsync(entity);
            }
            else
            {
                Set.Attach(entity).State = EntityState.Deleted;
            }
        }

        public virtual Task<int> SaveChangesAsync()
        {
            _logger?.LogInformation($"Método: { nameof(SaveChangesAsync) }()");

            return Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }

        #region private 

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

        #endregion
    }
}
