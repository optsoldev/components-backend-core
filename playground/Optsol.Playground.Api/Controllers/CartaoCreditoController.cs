using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
using Optsol.Components.Service.Response;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CartaoCreditoController : ApiControllerBase<CartaoCreditoEntity, CartaoCreditoViewModel, CartaoCreditoViewModel, InsertCartaoCreditoViewModel, UpdateCartaoCreditoViewModel>,
        IApiControllerBase<CartaoCreditoEntity, CartaoCreditoViewModel, CartaoCreditoViewModel, InsertCartaoCreditoViewModel, UpdateCartaoCreditoViewModel>
    {
        public CartaoCreditoController(
            ILogger<ApiControllerBase<CartaoCreditoEntity, CartaoCreditoViewModel, CartaoCreditoViewModel, InsertCartaoCreditoViewModel, UpdateCartaoCreditoViewModel>> logger,
            IResponseFactory responseFactory,
            IBaseServiceApplication<CartaoCreditoEntity, CartaoCreditoViewModel, CartaoCreditoViewModel, InsertCartaoCreditoViewModel, UpdateCartaoCreditoViewModel> applicationService)
            : base(logger, applicationService, responseFactory)
        {

        }

    }
}
