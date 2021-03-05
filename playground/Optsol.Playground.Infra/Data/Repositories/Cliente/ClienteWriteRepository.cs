using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entities;
using Optsol.Playground.Domain.Repositories.Cliente;
using System;

namespace Optsol.Playground.Infra.Data.Repositories.Cliente
{
    public class ClienteWriteRepository : Repository<ClienteEntity, Guid>, IClienteWriteRepository
    {
        public ClienteWriteRepository(CoreContext context, ILoggerFactory logger) 
            : base(context, logger)
        {
        }
    }
}
