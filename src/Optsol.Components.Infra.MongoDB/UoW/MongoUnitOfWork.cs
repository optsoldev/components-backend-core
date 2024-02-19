using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.MongoDB.Context;
using System;
using System.Threading.Tasks;
using Optsol.Components.Infra.UoW;

namespace Optsol.Components.Infra.MongoDB.UoW
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;

        private readonly ILogger _logger;

        public MongoContext Context { get; protected set; }

        public MongoUnitOfWork(MongoContext context, ILoggerFactory logger)
        {
            _logger = logger?.CreateLogger(nameof(MongoContext));
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
