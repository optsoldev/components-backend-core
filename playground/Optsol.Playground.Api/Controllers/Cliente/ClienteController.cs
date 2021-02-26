using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Security.Attributes;
using Optsol.Components.Service.Controllers;
using Optsol.Components.Service.Responses;
using Optsol.Playground.Application.Searchs;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;
using System.Threading.Tasks;

namespace Optsol.Playground.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public partial class ClienteController : ApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel, ClienteSearchDto>,
        IApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel, ClienteSearchDto>
    {
        public readonly IClienteServiceApplication _clienteServiceApplication;

        public ClienteController(
            ILogger<ApiControllerBase<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel, ClienteSearchDto>> logger,
            IResponseFactory responseFactory,
            IClienteServiceApplication clienteServiceApplication)
            : base(logger, clienteServiceApplication, responseFactory)
        {
            _clienteServiceApplication = clienteServiceApplication;
        }

        //[OptsolAuthorize("crud.buscar.id")]
        //public override Task<IActionResult> GetAllAsync()
        //{
        //    return base.GetAllAsync();
        //}
    }
}
