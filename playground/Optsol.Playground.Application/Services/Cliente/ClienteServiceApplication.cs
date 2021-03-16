using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Infra.UoW;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;
using Optsol.Playground.Domain.Repositories.Cliente;
using System;
using System.Threading.Tasks;

namespace Optsol.Playground.Application.Services.Cliente
{
    public class ClienteServiceApplication : BaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>, IClienteServiceApplication
    {
        protected readonly IClienteReadRepository _clienteReadRepository;
        protected readonly IClienteWriteRepository _clienteWriteRepository;

        public ClienteServiceApplication(
            IMapper mapper,
            ILoggerFactory logger,
            IUnitOfWork unitOfWork,
            IClienteWriteRepository clienteWriteRepository,
            IClienteReadRepository clienteReadRepository,
            NotificationContext notificationContext)
            : base(mapper, logger, unitOfWork, clienteReadRepository, clienteWriteRepository, notificationContext)
        {
            _clienteReadRepository = clienteReadRepository;
            _clienteWriteRepository = clienteWriteRepository;
        }

        public async Task<ClienteComCartoesViewModel> GetClienteComCartaoCreditoAsync(Guid id)
        {
            var clienteEntity = await _clienteReadRepository.BuscarClienteComCartaoCreditoAsync(id);
            return _mapper.Map<ClienteComCartoesViewModel>(clienteEntity);
        }

        public async Task InserirCartaoNoClienteAsync(InsertCartaoCreditoViewModel insertCartaoCreditoViewModel)
        {
            var clienteEntity = await _clienteReadRepository.GetByIdAsync(insertCartaoCreditoViewModel.ClienteId);

            var entity = _mapper.Map<CartaoCreditoEntity>(insertCartaoCreditoViewModel);
            clienteEntity.AdicionarCartao(entity);

            await _clienteWriteRepository.UpdateAsync(clienteEntity);

            await CommitAsync();
        }
    }
}
