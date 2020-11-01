using System;
using Optsol.Components.Application.Service;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application
{
    public interface IClienteServiceApplication : IBaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, ClienteViewModel, ClienteViewModel>
    {
    }
}
