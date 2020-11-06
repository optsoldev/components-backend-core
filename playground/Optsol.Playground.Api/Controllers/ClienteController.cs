using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
using Optsol.Components.Service.Response;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;
using Optsol.Playground.Domain.Repositories.Cliente;

namespace Optsol.Playground.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ClienteController : ApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>,
        IApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>
    {
        protected readonly IClienteServiceApplication _clienteServiceApplication;

        public ClienteController(
            ILogger<ApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>> logger,
            IResponseFactory responseFactory,
            IBaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel> applicationService,
            IClienteServiceApplication clienteServiceApplication)
            : base(logger, applicationService, responseFactory)
        {
            _clienteServiceApplication = clienteServiceApplication;
        }

        [HttpGet("{id}/cartaoCredito")]
        public async Task<IActionResult> GetClienteComCartaoCredito(Guid id)
        {
            var cliente = await _clienteServiceApplication.GetClienteComCartaoCredito(id);

            return Ok(_responseFactory.Create(cliente));
        }
    }
}
