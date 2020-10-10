using System.Data.Common;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Optsol.Sdk.Infra.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private ILogger _logger;
        // public TContext Context { get; protected set; }
        public DbContext Context { get; protected set; }

        public UnitOfWork(DbContext context, ILogger<UnitOfWork> logger)
        {
            _logger = logger;
            _logger?.LogInformation("Inicializando UnitOfWork");

            Context = context;
        }

        public Task<bool> CommitAsync()
        {
            _logger?.LogInformation($"Método: { nameof(CommitAsync) }() Retorno: bool");

            return Task.Factory.StartNew(() =>
            {
                return Context.SaveChanges() > 0;
            });
        }

        private void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    Context.Dispose();
                }
            }            
            disposed = true;
        }

        public void Dispose()
        {
            _logger?.LogInformation($"Método: { nameof(Dispose) }()");

            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
