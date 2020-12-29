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
    public class Repository<TEntity, TKey> : 
        IRepository<TEntity, TKey>, IDisposable
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

        public virtual Task<TEntity> GetByIdAsync(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TEntity).Name }");

            return Set.FindAsync(id).AsTask();
        }
        
        public virtual IAsyncEnumerable<TEntity> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IAsyncEnumerable<{ typeof(TEntity).Name }>");

            return Set.AsAsyncEnumerable();
        }

        public IAsyncEnumerable<TEntity> GetAllAsync<TSearch>(IRequestSearch<TSearch> requestSearch)
            where TSearch : class
        {
            var requestFilter = requestSearch.Search as ISearch<TEntity, TKey>;
            
            throw new NotImplementedException();
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ entity:{ entity.ToJson() } }})");
                
            return Set.AddAsync(entity).AsTask();
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ entity:{ entity.ToJson() } }})");
            
            var localEntity = Context.Set<TEntity>().Local?.Where(w => w.Id.Equals(entity.Id)).FirstOrDefault();
            var inLocal = localEntity != null;
            if(inLocal)
            {
                Context.Entry(localEntity).State = EntityState.Detached;
            }

            Set.Update(entity);
            
            return Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await Set.FindAsync(id);
            
            if(entity == null)
            {
                _logger?.LogError($"Método: { nameof(DeleteAsync) }({{ TKey:{ id.ToJson() } }}) Registro não encontrado");
                return;
            }
                

            await DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
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

        public virtual Task<int> SaveChanges()
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
