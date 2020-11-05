using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entidades;
using Optsol.Playground.Domain.Repositories.CartaoCredito;

namespace Optsol.Playground.Infra.Data.Repositories.CartaoCredito
{
    public class CartaoCreditoReadRepository : Repository<CartaoCreditoEntity, Guid>, ICartaoCreditoReadRepository
    {
        public CartaoCreditoReadRepository(DbContext context, ILogger<Repository<CartaoCreditoEntity, Guid>> logger) : base(context, logger)
        {
        }
    }
}
