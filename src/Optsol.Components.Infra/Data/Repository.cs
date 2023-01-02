using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Domain.Data;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Pagination;
using Optsol.Components.Infra.Data.Pagination;
using Optsol.Components.Infra.Data.Provider;
using Optsol.Components.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Data
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        private bool disposed;

        private readonly ILogger logger;

        private readonly ITenantProvider<TKey> tenantProvider;

        // ReSharper disable once MemberCanBePrivate.Global
        public CoreContext Context { get; protected set; }

        // ReSharper disable once MemberCanBeProtected.Global
        public DbSet<TEntity> Set { get; protected set; }

        public Repository(CoreContext context, ILoggerFactory logger, ITenantProvider<TKey> tenantProvider = null)
        {
            this.logger = logger.CreateLogger(nameof(Repository<TEntity, TKey>));
            this.logger?.LogInformation($"Inicializando Repository<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");

            Context = context ?? throw new DbContextNullException();
            Set = context.Set<TEntity>();

            this.tenantProvider = tenantProvider;
            ValidateTenantProvider();
        }

        private void ValidateTenantProvider()
        {
            TypeFilter filter = new(InterfaceFilter);

            var @interface = $"{typeof(ITenant<TKey>).Namespace}.{typeof(ITenant<TKey>).Name.Replace("`1", "")}";
            var repositoryInvalid = typeof(TEntity).FindInterfaces(filter, @interface).Any() && tenantProvider == null;
            if (!repositoryInvalid) return;
            
            logger?.LogError($"Essa entidade implementa ITenant, o ITenantProvider deve ser injetado.");
            throw new InvalidRepositoryException();
        }

        private static bool InterfaceFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
                return true;
            else
                return false;
        }

        public virtual Task<TEntity> GetByIdAsync(TKey id)
        {
            logger?.LogInformation($"Método: { nameof(GetByIdAsync) }( {{ id:{ id } }} ) Retorno: type { typeof(TEntity).Name }");

            return Set.FindAsync(id).AsTask();
        }

        public virtual Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids)
        {
            logger?.LogInformation($"Método: { nameof(GetByIdAsync) }( {{ ids:[{ string.Join(",", ids) }]}} ) Retorno: type { typeof(TEntity).Name }");

            return Set.Where(a => ids.Contains(a.Id)).AsAsyncEnumerable().AsyncEnumerableToEnumerable();
        }

        public virtual Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
        {
            logger?.LogInformation($"Método: { nameof(GetByIdAsync) }( {{ ids:[{ string.Join(",", ids) }]}} ) Retorno: type { typeof(TEntity).Name }");

            var querable = Set.AsQueryable();

            var hasInclude = includes != null;
            if (hasInclude)
            {
                querable = includes.Invoke(querable);
            }

            return querable.Where(a => ids.Contains(a.Id)).AsAsyncEnumerable().AsyncEnumerableToEnumerable();
        }

        public virtual Task<TEntity> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes)
        {
            logger?.LogInformation($"Método: { nameof(GetByIdAsync) }( {{ id:{ id } }} ) Retorno: type { typeof(TEntity).Name }");

            var querable = Set.AsQueryable();

            var hasInclude = Includes != null;
            if (hasInclude)
            {
                querable = Includes.Invoke(querable);
            }

            return querable.Where(w => w.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IAsyncEnumerable<{ typeof(TEntity).Name }>");

            return Set.AsAsyncEnumerable().AsyncEnumerableToEnumerable();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes)
        {
            logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IAsyncEnumerable<{ typeof(TEntity).Name }>");

            var querable = Set.AsQueryable();

            var hasInclude = Includes != null;
            if (hasInclude)
            {
                querable = Includes.Invoke(querable);
            }

            return querable.AsAsyncEnumerable().AsyncEnumerableToEnumerable();
        }

        public virtual Task<ISearchResult<TEntity>> GetAllAsync<TSearch>(ISearchRequest<TSearch> requestSearch)
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
            logger?.LogInformation($"Método: { nameof(InsertAsync) }( {{entity:{ entity.ToJson() }}} )");

            var entityIsITenant = entity is ITenant<TKey>;
            if (entityIsITenant)
            {
                logger?.LogInformation($"Executando SetTenantId({tenantProvider.TenantId}) em InsertAsync");
                ((ITenant<TKey>)entity).SetTenantId(tenantProvider.TenantId);
            }

            return Set.AddAsync(entity).AsTask();
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            logger?.LogInformation($"Método: { nameof(UpdateAsync) }( {{entity:{ entity.ToJson() }}} )");

            SetDetachedLocalEntity(entity);

            SetTenantIdFromTenantProvider(entity);

            Set.Update(entity);

            return Task.CompletedTask;
        }

        private void SetDetachedLocalEntity(TEntity entity)
        {
            var localEntity = Set.Local?.FirstOrDefault(localEntity => localEntity.Id.Equals(entity.Id));
            if (localEntity is null)
                return;

            Context.Entry(localEntity).State = EntityState.Detached;
        }

        private void SetTenantIdFromTenantProvider(TEntity entity)
        {
            if (entity is not ITenant<TKey> tenant)
                return;

            logger?.LogInformation($"Executando SetTenantId({tenantProvider.TenantId}) em UpdateAsync");
            tenant.SetTenantId(tenantProvider.TenantId);

        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await Set.FindAsync(id);

            var entityNotFound = entity == null;
            if (entityNotFound)
            {
                logger?.LogError($"Método: { nameof(DeleteAsync) }({{ TKey:{ id.ToJson() } }}) Registro não encontrado");
                return;
            }

            await DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            logger?.LogInformation($"Método: { nameof(DeleteAsync) }( {{entity:{ entity.ToJson() }}} )");

            var entityFound = entity != null;
            var entityIsDeletable = entity is IDeletable;

            if (entityFound && entityIsDeletable)
            {
                ((IDeletable)entity).Delete();
                await UpdateAsync(entity);
            }
            else
            {
                if (entity != null) Set.Attach(entity).State = EntityState.Deleted;
            }
        }

        public virtual Task<int> SaveChangesAsync()
        {
            logger?.LogInformation($"Método: { nameof(SaveChangesAsync) }()");

            return Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            logger?.LogInformation($"Método: { nameof(Dispose) }()");

            if (!disposed && disposing)
            {
                Context.Dispose();
            }
            disposed = true;
        }

        private static async Task<ISearchResult<TEntity>> CreateSearchResult(IQueryable<TEntity> query, uint page, uint? pageSize)
        {
            var searchResult = new SearchResult<TEntity>(page, pageSize)
            {
                Total = await query.CountAsync()
            };

            query = ApplyPagination(query, page, pageSize);

            searchResult.Items = await query.AsAsyncEnumerable().AsyncEnumerableToEnumerable();
            searchResult.TotalItems = searchResult.Items.Count();

            return searchResult;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected static IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, uint page, uint? pageSize)
        {
            var skip = --page * (pageSize ?? 0);

            query = query.Skip(skip.ToInt());

            if (pageSize.HasValue)
            {
                query = query.Take(pageSize.Value.ToInt());
            }

            return query;
        }

        private static IQueryable<TEntity> ApplySearch(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> search = null)
        {
            var searchIsNotNull = search != null;
            if (searchIsNotNull)
            {
                query = query.Where(search);
            }

            return query;
        }

        private static IQueryable<TEntity> ApplyInclude(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var includesIsNotNull = includes != null;
            if (includesIsNotNull)
            {
                query = includes(query);
            }

            return query;
        }

        private static IQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
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
