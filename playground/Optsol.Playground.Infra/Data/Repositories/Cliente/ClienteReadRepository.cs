using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Clientes.Repositories;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Infra.Data.Repositories.Cliente
{
    public class ClienteReadRepository : Repository<ClientePessoaFisica, Guid>, IClientePessoaFisicaReadRepository
    {
        public ClienteReadRepository(CoreContext context, ILoggerFactory logger) 
            : base(context, logger)
        {
        }

        public async Task<ClientePessoaFisica> BuscarClienteComCartaoCreditoAsync(Guid id)
        {
            var entity = await Set.Include(x => x.Cartoes).FirstAsync(x => x.Id == id);
            return entity;
        }

        public IAsyncEnumerable<ClientePessoaFisica> BuscarClientesComCartaoCreditoAsync()
        {
            return Set.Include(x => x.Cartoes).AsAsyncEnumerable();
        }
    }
}
