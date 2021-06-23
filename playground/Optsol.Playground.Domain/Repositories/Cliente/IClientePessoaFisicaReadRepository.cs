using Optsol.Components.Domain.Data;
using Optsol.Playground.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optsol.Playground.Domain.Repositories.Cliente
{
    public interface IClientePessoaFisicaReadRepository : IReadRepository<ClientePessoaFisicaEntity, Guid>
    {
        Task<ClientePessoaFisicaEntity> BuscarClienteComCartaoCreditoAsync(Guid id);

        IAsyncEnumerable<ClientePessoaFisicaEntity> BuscarClientesComCartaoCreditoAsync();
    }
}
