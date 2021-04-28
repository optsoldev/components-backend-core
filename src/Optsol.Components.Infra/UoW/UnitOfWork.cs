using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using System;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;

        private readonly ILogger _logger;

        public CoreContext Context { get; protected set; }

        public UnitOfWork(CoreContext context, ILoggerFactory logger)
        {
            _logger = logger?.CreateLogger(nameof(UnitOfWork));
            _logger?.LogInformation("Inicializando UnitOfWork");

            Context = context;
        }

        public Task<int> CommitAsync()
        {
            _logger?.LogInformation($"Método: { nameof(CommitAsync) }() Retorno: bool");

            return Task.FromResult(Context.SaveChanges());
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
