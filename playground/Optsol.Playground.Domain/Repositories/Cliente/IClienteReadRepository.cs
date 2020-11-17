using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Domain.Repositories.Cliente
{
    public interface IClienteReadRepository : IReadRepository<ClienteEntity, Guid>
    {
        Task<ClienteEntity> GetClienteComCartaoCreditoAsync(Guid id);
        IAsyncEnumerable<ClienteEntity> GetClientesComCartaoCreditoAsync();
    }
}
