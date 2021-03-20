using Microsoft.AspNetCore.Mvc;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using System;
using System.Threading.Tasks;

namespace Optsol.Playground.Api.Controllers
{
    public partial class ClienteController
    {
        [HttpGet("{id}/cartao-credito")]
        public async Task<IActionResult> GetClienteComCartaoCredito(Guid id)
        {
            var cliente = await _clienteServiceApplication.GetClienteComCartaoCreditoAsync(id);

            return CreateResult(cliente);
        }

        [HttpPost("cartao-credito")]
        public async Task<IActionResult> InserirCartaoNoCliente([FromBody] InsertCartaoCreditoViewModel insertCartaoCreditoViewModel)
        {
            await _clienteServiceApplication.InserirCartaoNoClienteAsync(insertCartaoCreditoViewModel);

            return CreateResult();
        }
    }
}
