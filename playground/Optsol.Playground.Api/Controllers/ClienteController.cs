using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
using Optsol.Components.Service.Response;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ClienteController : ApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>,
        IApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>
    {
        public ClienteController(
            ILogger<ApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>> logger,
            IResponseFactory responseFactory,
            IBaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel> applicationService)
            : base(logger, applicationService, responseFactory)
        {

        }

        // [HttpGet("{id}")]
        // public override Task<IActionResult> GetByIdAsync<TViewModel>(Guid id)
        // {
        //     return base.GetByIdAsync<ClienteViewModel>(id);
        // }
    }
}
