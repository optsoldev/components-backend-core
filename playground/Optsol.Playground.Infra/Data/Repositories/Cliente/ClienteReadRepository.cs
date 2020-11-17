using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Infra.Data.Repositories.Cliente
{
    public class ClienteReadRepository : Repository<ClienteEntity, Guid>, IClienteReadRepository
    {
        public ClienteReadRepository(DbContext context, ILogger<Repository<ClienteEntity, Guid>> logger) : base(context, logger)
        {
        }

        public async Task<ClienteEntity> GetClienteComCartaoCredito(Guid id)
        {
            var entity = await Set.Include(x => x.Cartoes).FirstAsync(x => x.Id == id);
            return entity;
        }

        public IAsyncEnumerable<ClienteEntity> GetClientesComCartaoCredito()
        {
            return Set.Include(x => x.Cartoes).AsAsyncEnumerable();
        }
    }
}
