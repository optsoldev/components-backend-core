using Optsol.Components.Domain.Data;
using Optsol.Playground.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optsol.Playground.Domain.Repositories.Cliente
{
    public interface IClienteReadRepository : IReadRepository<ClienteEntity, Guid>
    {
        Task<ClienteEntity> BuscarClienteComCartaoCreditoAsync(Guid id);
        IAsyncEnumerable<ClienteEntity> BuscarClientesComCartaoCreditoAsync();
    }
}
