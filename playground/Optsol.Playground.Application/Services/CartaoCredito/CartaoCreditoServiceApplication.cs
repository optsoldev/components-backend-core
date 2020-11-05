using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application.Services.CartaoCredito
{
    public class CartaoCreditoServiceApplication : BaseServiceApplication<CartaoCreditoEntity, CartaoCreditoViewModel, CartaoCreditoViewModel, InsertCartaoCreditoViewModel, UpdateCartaoCreditoViewModel>,
        ICartaoCreditoServiceApplication
    {
        public CartaoCreditoServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<CartaoCreditoEntity, CartaoCreditoViewModel, CartaoCreditoViewModel, InsertCartaoCreditoViewModel, UpdateCartaoCreditoViewModel>> logger,
            IServiceResultFactory serviceResultFactory,
            IUnitOfWork unitOfWork,
            IReadRepository<CartaoCreditoEntity, Guid> readRepository,
            IWriteRepository<CartaoCreditoEntity, Guid> writeRepository)
            : base(mapper, logger, serviceResultFactory, unitOfWork, readRepository, writeRepository)
        {
        }
    }
}
