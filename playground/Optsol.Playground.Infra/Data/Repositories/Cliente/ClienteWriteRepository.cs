using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entities;
using System;
using Optsol.Playground.Domain.Clientes.Repositories;

namespace Optsol.Playground.Infra.Data.Repositories.Cliente
{
    public class ClienteWriteRepository : Repository<ClientePessoaFisica, Guid>, IClientePessoaFisicaWriteRepository
    {
        public ClienteWriteRepository(CoreContext context, ILoggerFactory logger) 
            : base(context, logger)
        {
        }
    }
}
