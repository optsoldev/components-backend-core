using Optsol.Components.Domain.Data;
using Optsol.Playground.Domain.Entities;
using System;

namespace Optsol.Playground.Domain.Repositories.Cliente
{
    public interface IClienteWriteRepository : IWriteRepository<ClienteEntity, Guid>
    {
    }
}
