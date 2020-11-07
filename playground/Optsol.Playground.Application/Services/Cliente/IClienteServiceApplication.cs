using System;
using System.Threading.Tasks;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application.Services.Cliente
{
    public interface IClienteServiceApplication : IBaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>
    {
        Task<ServiceResult<ClienteComCartoesViewModel>> GetClienteComCartaoCredito(Guid id);
    }
}
