using System;
using Optsol.Components.Application.Service;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application.Services
{
    public interface IClienteServiceApplication : IBaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, ClienteViewModel, ClienteViewModel>
    {
    }
}
