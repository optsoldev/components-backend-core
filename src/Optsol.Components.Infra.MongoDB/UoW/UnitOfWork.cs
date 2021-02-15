using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.MongoDB.Context;
using System;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.MongoDB.UoW
{
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        private bool disposed = false;
        private readonly ILogger _logger;

        public MongoContext Context { get; protected set; }

        public MongoUnitOfWork(MongoContext context, ILogger<MongoUnitOfWork> logger)
        {
            _logger = logger;
            _logger?.LogInformation("Inicializando UnitOfWork");

            Context = context;
        }

        public async Task<int> CommitAsync()
        {
            _logger?.LogInformation($"Método: { nameof(CommitAsync) }() Retorno: bool");

            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _logger?.LogInformation($"Método: { nameof(Dispose) }()");

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
