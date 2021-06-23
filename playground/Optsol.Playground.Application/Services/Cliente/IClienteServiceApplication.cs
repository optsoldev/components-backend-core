using Optsol.Components.Application.Services;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Optsol.Playground.Application.Services.Cliente
{
    public interface IClienteServiceApplication : IBaseServiceApplication<ClientePessoaFisicaEntity>
    {
        Task<ClienteComCartoesViewModel> GetClienteComCartaoCreditoAsync(Guid id);

        Task InserirCartaoNoClienteAsync(InsertCartaoCreditoViewModel insertCartaoCreditoViewModel);
    }
}
