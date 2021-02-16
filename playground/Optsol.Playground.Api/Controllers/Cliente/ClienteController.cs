using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Service.Controllers;
using Optsol.Components.Service.Responses;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public partial class ClienteController : ApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>,
        IApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>
    {
        public readonly IClienteServiceApplication _clienteServiceApplication;

        public ClienteController(
            ILogger<ApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>> logger,
            IResponseFactory responseFactory,
            IClienteServiceApplication clienteServiceApplication)
            : base(logger, clienteServiceApplication, responseFactory)
        {
            _clienteServiceApplication = clienteServiceApplication;
        }
    }
}
