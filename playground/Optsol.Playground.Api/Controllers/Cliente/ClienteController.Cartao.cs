using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Optsol.Playground.Application.ViewModels.CartaoCredito;

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

        [HttpPost("cartaoCredito")]
        public async Task<IActionResult> InserirCartaoNoCliente([FromBody] InsertCartaoCreditoViewModel insertCartaoCreditoViewModel)
        {
            var response = await _clienteServiceApplication.InserirCartaoNoClienteAsync(insertCartaoCreditoViewModel);

            return Ok(_responseFactory.Create(response));
        }
    }
}
