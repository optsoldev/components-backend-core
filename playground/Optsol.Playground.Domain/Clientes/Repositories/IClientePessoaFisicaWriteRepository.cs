using System;
using Optsol.Components.Domain.Data;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Domain.Clientes.Repositories;

public interface IClientePessoaFisicaWriteRepository : IWriteRepository<ClientePessoaFisica, Guid>
{
}