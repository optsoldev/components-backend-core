using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entities;
using Optsol.Playground.Domain.Repositories.Cliente;

namespace Optsol.Playground.Infra.Data.Repositories.Cliente
{
    public class ClienteReadRepository : Repository<ClientePessoaFisicaEntity, Guid>, IClientePessoaFisicaReadRepository
    {
        public ClienteReadRepository(CoreContext context, ILoggerFactory logger) 
            : base(context, logger)
        {
        }

        public async Task<ClientePessoaFisicaEntity> BuscarClienteComCartaoCreditoAsync(Guid id)
        {
            var entity = await Set.Include(x => x.Cartoes).FirstAsync(x => x.Id == id);
            return entity;
        }

        public IAsyncEnumerable<ClientePessoaFisicaEntity> BuscarClientesComCartaoCreditoAsync()
        {
            return Set.Include(x => x.Cartoes).AsAsyncEnumerable();
        }
    }
}
