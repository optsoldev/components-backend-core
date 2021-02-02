using System;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Domain.Repositories.Cliente
{
    public interface IClienteWriteRepository : IMontoWriteRepository<ClienteEntity, Guid>
    {
    }
}
