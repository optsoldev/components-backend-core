using System;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Infra.Data.Repositories.Cliente
{
    public interface IClienteWriteRepository : IWriteRepository<ClienteEntity, Guid>
    {
    }
}
