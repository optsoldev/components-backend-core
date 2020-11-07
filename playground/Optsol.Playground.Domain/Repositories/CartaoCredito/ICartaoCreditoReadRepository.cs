using System;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Domain.Repositories.CartaoCredito
{
    public interface ICartaoCreditoReadRepository : IReadRepository<CartaoCreditoEntity, Guid>
    {
    }
}