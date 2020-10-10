using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Sdk.Domain;
using Optsol.Sdk.Shared.Exceptions;
using Optsol.Sdk.Shared.Extensions;

namespace Optsol.Sdk.Infra.Data
{
    public class Repository<TEntity, TKey> : 
        IRepository<TEntity, TKey>
        where TEntity: class, IAggregateRoot<TKey>
    {
        public DbContext Context { get; protected set; }
        public DbSet<TEntity> Set { get; protected set; }

        private ILogger _logger;

        public Repository(DbContext context, ILogger<Repository<TEntity, TKey>> logger)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Repository<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");

            Context = context ?? throw new DbContextNullException();
            this.Set = context.Set<TEntity>();
        }

        #region IReadRepository

        public Task<TEntity> GetById(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(GetById) }({{ id:{ id } }}) Retorno: type { typeof(TEntity).Name }");

            return Set.FindAsync(id).AsTask();
        }
        
        public IAsyncEnumerable<TEntity> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IAsyncEnumerable<{ typeof(TEntity).Name }>");

            return Set.AsAsyncEnumerable();
        }


        #endregion

        #region IWriteRepository

        public Task InsertAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ entity:{ entity.ToJson() } }})");

            return Set.AddAsync(entity).AsTask();
        }

        public Task UpdateAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ entity:{ entity.ToJson() } }})");

            return Task.Factory.StartNew(() => 
            {
                Set.Update(entity);
            });
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await Set.FindAsync(id);
            await DeleteAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ entity:{ entity.ToJson() } }})");

            if(entity != null && entity is IDeletable)
            {
                ((IDeletable)entity).Delete();
                await UpdateAsync(entity);
            }
            else
            {
                Set.Attach(entity).State = EntityState.Deleted;
            }
        }

        public Task<int> SaveChanges()
        {
            _logger?.LogInformation($"Método: { nameof(SaveChanges) }()");

            return Context.SaveChangesAsync();
        }

        #endregion
    }
}
