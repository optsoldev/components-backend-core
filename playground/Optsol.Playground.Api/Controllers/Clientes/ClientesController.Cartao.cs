using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optsol.Components.Infra.Security.AzureB2C.Security.Attributes;
using Optsol.Playground.Application.ViewModels.CartaoCredito;

namespace Optsol.Playground.Api.Controllers.Clientes;

[Authorize]
public partial class ClientesController
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
