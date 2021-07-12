using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Service.Controllers;
using Optsol.Components.Service.Responses;
using Optsol.Playground.Application.Searchs;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public partial class ClienteController : ApiControllerBase<ClientePessoaFisicaEntity, ClienteRequest, ClienteResponse, ClienteSearchDto>,
        IApiControllerBase<ClientePessoaFisicaEntity, ClienteRequest, ClienteResponse, ClienteSearchDto>
    {
        public readonly IClienteServiceApplication _clienteServiceApplication;

        public ClienteController(
            ILoggerFactory logger,
            IResponseFactory responseFactory,
            IClienteServiceApplication clienteServiceApplication)
            : base(logger, clienteServiceApplication, responseFactory)
        {
            _clienteServiceApplication = clienteServiceApplication;
            _clienteServiceApplication.Includes = clientes => clientes.Include(x => x.Cartoes);
        }
    }
}
