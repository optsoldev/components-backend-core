using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using System;
using System.Threading.Tasks;
using Optsol.Components.Infra.Security.AzureB2C.Security.Attributes;

namespace Optsol.Playground.Api.Controllers
{
    [Authorize]
    public partial class ClienteController
    {
        [OptsolAuthorize("cliente.buscar")]
        [HttpGet("{id}/cartao-credito")]
        public async Task<IActionResult> GetClienteComCartaoCredito(Guid id)
        {
            var cliente = await _clienteServiceApplication.GetClienteComCartaoCreditoAsync(id);

            return CreateResult(cliente);
        }

        [HttpPost("cartao-credito")]
        public async Task<IActionResult> InserirCartaoNoCliente([FromBody] CartaoCreditoRequest insertCartaoCreditoViewModel)
        {
            await _clienteServiceApplication.InserirCartaoNoClienteAsync(insertCartaoCreditoViewModel);

            return CreateResult();
        }
    }
}
