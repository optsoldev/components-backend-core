using Microsoft.Extensions.Logging;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.ElasticSearch.Context;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.ElasticSearch.Repositories
{
    public class ElasticRepository<TEntity, TKey> : 
        IElasticRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {

        private readonly ILogger _logger;

        public ElasticContext Context { get; protected set; }

        public ElasticRepository(ElasticContext context, ILoggerFactory logger)
        {
            _logger = logger?.CreateLogger(nameof(ElasticRepository<TEntity, TKey>));
            _logger?.LogInformation($"Inicializando ElasticRepository<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");

            Context = context ?? throw new ElasticContextNullException();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }( {{id:{ id }}} ) Retorno: type { typeof(TEntity).Name }");

            var entity = await Context.ElasticClient.GetAsync<TEntity>(id.ToString());

            return entity.Source;
        }

        public async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ids:[{ string.Join(",", ids) }}}]) Retorno: type { typeof(TEntity).Name }");

            var filters = ids.Select(s => s.ToString());

            var entities = await Context
                .ElasticClient
                .MultiGetAsync(source => source.GetMany<TEntity>(filters, (g, id) => g.Index(null)));

            return entities.SourceMany<TEntity>(filters);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TEntity).Name }>");

            var entities = await Context.ElasticClient.SearchAsync<TEntity>();

            return entities.Documents;
        }

        public Task InsertAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(InsertAsync) }( {{entity:{ entity.ToJson()}}} )");

            Context.AddCommand(() => Context.ElasticClient.IndexDocumentAsync(entity));

            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }( {{entity:{ entity.ToJson()}}} )");

            Context.AddCommand(() => Context.ElasticClient.UpdateAsync<TEntity>(entity, update => update.Doc(entity).DocAsUpsert()));

            return Task.CompletedTask;
        }

        public Task DeleteAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }( {{entity:{ entity.ToJson()}}} )");

            return DeleteAsync(entity.Id);
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);

            var entityNotFound = entity == null;
            if (entityNotFound)
            {
                _logger?.LogError($"Método: { nameof(DeleteAsync) }( {{id:{ id }}} ) Registro não encontrado");
                return;
            }

            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }( {{entity:{ entity.ToJson()}}} )");

            Context.AddCommand(() => Context.ElasticClient.DeleteAsync<TEntity>(id.ToString()));
        }


        public virtual Task<int> SaveChangesAsync()
        {
            _logger?.LogInformation($"Método: { nameof(SaveChangesAsync) }()");

            return Context.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
