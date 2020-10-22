using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
using Optsol.Components.Service.Response;
using Optsol.Playground.Application.ViewModels;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Api.Controllers
{
    // [ApiController]
    // [Route("api/[Controller]")]
    // public class ClienteController : ApiControllerBase<ClienteEntity, Guid>, IApiControllerBase<ClienteEntity, Guid>
    // {
    //     public ClienteController(ILogger<ApiControllerBase<ClienteEntity, Guid>> logger, 
    //         IResponseFactory responseFactory, 
    //         IBaseServiceApplication<ClienteEntity, Guid> applicationService) 
    //         : base(logger, responseFactory, applicationService)
    //     {
            
    //     }

    //     [HttpGet("{id}")]
    //     public override Task<IActionResult> GetByIdAsync<TViewModel>(Guid id)
    //     {
    //         return base.GetByIdAsync<ClienteViewModel>(id);
    //     }
    // }
}
