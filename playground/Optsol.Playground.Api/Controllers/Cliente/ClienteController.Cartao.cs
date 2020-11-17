using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Optsol.Playground.Api.Controllers
{
    public partial class ClienteController
    {

        [HttpGet("{id}/cartaoCredito")]
        public async Task<IActionResult> GetClienteComCartaoCredito(Guid id)
        {
            var cliente = await _clienteServiceApplication.GetClienteComCartaoCreditoAsync(id);
        
            return Ok(_responseFactory.Create(cliente));
        }
    }
}
