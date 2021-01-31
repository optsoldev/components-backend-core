using System;
using System.Threading.Tasks;
using Optsol.Components.Application.Results;
using Optsol.Components.Application.Services;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Application.Services.Cliente
{
    public interface IClienteServiceApplication : IBaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>
    {
        Task<ServiceResult<ClienteComCartoesViewModel>> GetClienteComCartaoCreditoAsync(Guid id);
        Task<ServiceResult> InserirCartaoNoClienteAsync(InsertCartaoCreditoViewModel insertCartaoCreditoViewModel);
    }
}
