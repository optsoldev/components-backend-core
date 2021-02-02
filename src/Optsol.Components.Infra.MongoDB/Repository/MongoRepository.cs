using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.MongoDB.Repository
{
    public class MongoRepository<TEntity, TKey>
        : IMongoRepository<TEntity, TKey>, IDisposable
        where TEntity : class, IAggregateRoot<TKey>
    {
        private readonly ILogger _logger;

        public MongoContext Context { get; protected set; }

        public IMongoCollection<TEntity> Set { get; protected set; }

        public MongoRepository(MongoContext context, ILogger<MongoRepository<TEntity, TKey>> logger)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando MongoRepository<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");

            Context = context ?? throw new MongoContextNullException();
            Set = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async IAsyncEnumerable<TEntity> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IAsyncEnumerable<{ typeof(TEntity).Name }>");

            var entities = await Set.FindAsync(Builders<TEntity>.Filter.Empty);

            foreach (var entity in entities.ToEnumerable())
                yield return entity;
        }

        public Task<SearchResult<TEntity>> GetAllAsync<TSearch>(RequestSearch<TSearch> requestSearch)
            where TSearch : class
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TEntity).Name }");

            var entity = await Set.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));

            return entity.SingleOrDefault();
        }

        public Task InsertAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ entity:{ entity.ToJson() } }})");

            Context.AddCommand(() => Set.InsertOneAsync(entity));

            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ entity:{ entity.ToJson() } }})");

            Context.AddCommand(() => Set.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity));

            return Task.CompletedTask;
        }

        public Task DeleteAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ entity:{ entity.ToJson() } }})");

            Context.AddCommand(() => Set.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id)));

            return Task.CompletedTask;
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);

            var entityNotFound = entity == null;
            if (entityNotFound)
            {
                _logger?.LogError($"Método: { nameof(DeleteAsync) }({{ TKey:{ id.ToJson() } }}) Registro não encontrado");
                return;
            }

            await DeleteAsync(entity);
        }

        public Task<int> SaveChanges()
        {
            _logger?.LogInformation($"Método: { nameof(SaveChanges) }()");

            return Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
