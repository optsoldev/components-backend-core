using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Security.AzureB2C.Security.Attributes;
using Optsol.Components.Service.Controllers;
using Optsol.Components.Service.Responses;
using Optsol.Playground.Application.Searchs;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Api.Controllers.Clientes;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[Controller]")]
public partial class ClientesController : ApiControllerBase<ClientePessoaFisica, ClienteRequest, ClienteResponse, ClienteSearchDto>
{
    private readonly IClienteServiceApplication clienteServiceApplication;

    public ClientesController(
        ILoggerFactory logger,
        IResponseFactory responseFactory,
        IClienteServiceApplication clienteServiceApplication)
        : base(logger, clienteServiceApplication, responseFactory)
    {
        this.clienteServiceApplication = clienteServiceApplication;
        this.clienteServiceApplication.Includes = clientes => clientes.Include(x => x.Cartoes);
    }
    
    [OptsolAuthorize("ClaimTeste", "ClaimTeste2")]
    public override Task<IActionResult> GetByIdAsync(Guid id)
    {
        return base.GetByIdAsync(id);
    }
}