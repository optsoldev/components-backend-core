using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.ElasticSearch.Context;
using System;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.ElasticSearch.UoW
{
    public class ElasticUnitOfWork : IElasticUnitOfWork
    {
        private bool _disposed = false;
        private readonly ILogger _logger;

        public ElasticContext Context { get; protected set; }

        public ElasticUnitOfWork(ElasticContext context, ILoggerFactory logger)
        {
            _logger = logger?.CreateLogger(nameof(ElasticContext));
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
