using System;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Domain.Repositories
{
    public interface IClienteReadRepository : IReadRepository<ClienteEntity, Guid>
    {
    }
}

