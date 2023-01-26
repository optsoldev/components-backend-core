using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using Optsol.Components.Domain.Pagination;
using Optsol.Components.Infra.Data.Pagination;

namespace Optsol.Components.Infra.MongoDB.Repositories
{
    public class MongoRepository<TEntity, TKey> : IMongoRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        private bool _disposed;

        protected readonly ILogger _logger;

        public MongoContext Context { get; protected set; }

        public IMongoCollection<TEntity> Set { get; protected set; }

        public MongoRepository(MongoContext context, ILoggerFactory logger)
        {
            _logger = logger?.CreateLogger(nameof(MongoRepository<TEntity, TKey>));
            _logger?.LogInformation($"Inicializando MongoRepository<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");

            Context = context ?? throw new MongoContextNullException();
            Set = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public TEntity GetById(TKey id)
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IDeletable)))
            {
                var byIdDef = Builders<TEntity>.Filter.Eq(q => q.Id, id);
                var deletableDef = Builders<TEntity>.Filter.Eq("DeletedDate.Date", BsonNull.Value);
                var filterDef = Builders<TEntity>.Filter.And(byIdDef, deletableDef);
                return Set.Find(filterDef).FirstOrDefault();
            }

            return Set.Find(f => f.Id.Equals(id)).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetAllByIds(params TKey[] ids)
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IDeletable)))
            {
                var byIdDef = Builders<TEntity>.Filter.In(q => q.Id, ids);
                var deletableDef = Builders<TEntity>.Filter.Eq("DeletedDate.Date", BsonNull.Value);
                var filterDef = Builders<TEntity>.Filter.And(byIdDef, deletableDef);

                return Set.Find(filterDef).ToEnumerable();
            }

            return Set.Find(f => ids.Contains(f.Id)).ToEnumerable();
        }

        public IEnumerable<TEntity> GetAll()
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IDeletable)))
            {
                var deletableDef = Builders<TEntity>.Filter.Eq("DeletedDate.Date", BsonNull.Value);
                return Set.Find(deletableDef).ToEnumerable();
            }

            return Set.AsQueryable().ToEnumerable();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filterExpression)
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IDeletable)))
            {
                var expressionDef = Builders<TEntity>.Filter.Where(filterExpression);
                var deletableDef = Builders<TEntity>.Filter.Eq("DeletedDate.Date", BsonNull.Value);
                var filterDef = Builders<TEntity>.Filter.And(expressionDef, deletableDef);

                return Set.Find(filterDef).ToEnumerable();
            }

            return Set.Find(filterExpression).ToEnumerable();
        }

        public ISearchResult<TEntity> GetAll<TSearch>(ISearchRequest<TSearch> searchRequest) where TSearch : class
        {
          var search = searchRequest.Search as Pagination.ISearch<TEntity>;
            var orderBy = searchRequest.Search as Pagination.IOrderBy<TEntity>;

            var page = searchRequest.Page > 0 ? searchRequest.Page : 1;
            var pageSize = searchRequest.PageSize is not null && searchRequest.PageSize.Value > 0
                ? searchRequest.PageSize.Value
                : 10;
        
            var countFacet = AggregateFacet.Create("countFacet",
                PipelineDefinition<TEntity, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<TEntity>()
                }));

            var sortDef = orderBy is null
                ? Builders<TEntity>.Sort.Descending(d => d.CreatedDate)
                : orderBy.GetOrderBy().Invoke(Builders<TEntity>.Sort);

            var dataFacet = AggregateFacet.Create("dataFacet",
                PipelineDefinition<TEntity, TEntity>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(sortDef),
                    PipelineStageDefinitionBuilder.Skip<TEntity>((int) ((page - 1) * pageSize)),
                    PipelineStageDefinitionBuilder.Limit<TEntity>((int) pageSize)
                }));

            var filterDef = GetFilterDef(search);

            var aggregation = Set.Aggregate()
                .Match(filterDef)
                .Facet(countFacet, dataFacet)
                .ToList();

            var count = aggregation.First()
                .Facets.First(x => x.Name.Equals("countFacet"))
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name.Equals("dataFacet"))
                .Output<TEntity>();

            return new SearchResult<TEntity>(page, searchRequest.PageSize)
                .SetPaginatedItems(data)
                .SetTotalCount((int)count);
        }
        
        private static FilterDefinition<TEntity> GetFilterDef(Pagination.ISearch<TEntity> search)
        {
            var defaultDef = Builders<TEntity>.Filter.Empty;

            var isDeletable = (typeof(TEntity).GetInterfaces().Contains(typeof(IDeletable)));
            var deletableDef = Builders<TEntity>.Filter.Eq("DeletedDate.Date", BsonNull.Value);

            if (search is null)
            {
                return isDeletable ? Builders<TEntity>.Filter.And(defaultDef, deletableDef) : defaultDef;
            }

            var searchDef = search.GetSearcher().Invoke(Builders<TEntity>.Filter);

            return isDeletable ? Builders<TEntity>.Filter.And(searchDef, deletableDef) : searchDef;
        }

        
        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }( {{id:{ id }}} ) Retorno: type { typeof(TEntity).Name }");

            var entity = await Set.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));

            return entity.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ids:[{ string.Join(",", ids) }}}]) Retorno: type { typeof(TEntity).Name }");

            var entities = await Set.FindAsync(Builders<TEntity>.Filter.All("_id", ids));

            return await entities.ToListAsync().AsyncCursorToAsyncEnumerable();
        }

        public Task<ISearchResult<TEntity>> GetAllAsync<TSearch>(ISearchRequest<TSearch> requestSearch) where TSearch : class
        {
            return Task.FromResult(GetAll(requestSearch));
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IAsyncEnumerable<{ typeof(TEntity).Name }>");

            var entities = await Set.FindAsync(Builders<TEntity>.Filter.Empty);

            return await entities.ToListAsync().AsyncCursorToAsyncEnumerable();
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(InsertAsync) }( {{entity:{ GenericExtensions.ToJson(entity)}}} )");

            Context.AddCommand(() => Set.InsertOneAsync(entity));

            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }( {{entity:{ GenericExtensions.ToJson(entity)}}} )");

            Context.AddCommand(() => Set.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity));

            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }( {{entity:{ GenericExtensions.ToJson(entity)}}} )");

            Context.AddCommand(() => Set.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id)));

            return Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);

            var entityNotFound = entity == null;
            if (entityNotFound)
            {
                _logger?.LogError($"Método: { nameof(DeleteAsync) }( {{id:{ id }}} ) Registro não encontrado");
                return;
            }

            await DeleteAsync(entity);
        }

        public virtual Task<int> SaveChangesAsync()
        {
            _logger?.LogInformation($"Método: { nameof(SaveChangesAsync) }()");

            return Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _logger?.LogInformation($"Método: { nameof(Dispose) }()");

            if (!_disposed && disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }
    }
}
