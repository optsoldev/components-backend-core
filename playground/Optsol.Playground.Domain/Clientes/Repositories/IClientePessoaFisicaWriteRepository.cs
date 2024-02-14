using System;
using Optsol.Components.Domain.Repositories;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Domain.Clientes.Repositories;

public interface IClientePessoaFisicaWriteRepository : IWriteRepository<ClientePessoaFisica, Guid>
{
}