using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Components.Domain.Data;
using Optsol.Components.Domain.Repositories;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Domain.Clientes.Repositories;

public interface IClientePessoaFisicaReadRepository : IReadRepository<ClientePessoaFisica, Guid>
{
    Task<ClientePessoaFisica> BuscarClienteComCartaoCreditoAsync(Guid id);

    IAsyncEnumerable<ClientePessoaFisica> BuscarClientesComCartaoCreditoAsync();
}