using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Results;
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
    public class ClienteServiceApplication : BaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>,
        IClienteServiceApplication
    {
        protected readonly IClienteReadRepository _clienteReadRepository;
        protected readonly IClienteWriteRepository _clienteWriteRepository;

        public ClienteServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>> logger,
            IServiceResultFactory serviceResultFactory,
            IUnitOfWork unitOfWork,
            IClienteWriteRepository clienteWriteRepository,
            IClienteReadRepository clienteReadRepository,
            NotificationContext notificationContext)
            : base(mapper, logger, serviceResultFactory, unitOfWork, clienteReadRepository, clienteWriteRepository, notificationContext)
        {
            _clienteReadRepository = clienteReadRepository;
            _clienteWriteRepository = clienteWriteRepository;
        }

        public async Task<ServiceResult<ClienteComCartoesViewModel>> GetClienteComCartaoCreditoAsync(Guid id)
        {
            var clienteEntity = await _clienteReadRepository.BuscarClienteComCartaoCreditoAsync(id);
            var clienteViewModel = _mapper.Map<ClienteComCartoesViewModel>(clienteEntity);

            return _serviceResultFactory.Create(clienteViewModel);
        }

        public async Task<ServiceResult> InserirCartaoNoClienteAsync(InsertCartaoCreditoViewModel insertCartaoCreditoViewModel)
        {
            var serviceResult = _serviceResultFactory.Create();

            var clienteEntity = await _clienteReadRepository.GetByIdAsync(insertCartaoCreditoViewModel.ClienteId);

            var entity = _mapper.Map<CartaoCreditoEntity>(insertCartaoCreditoViewModel);
            clienteEntity.AdicionarCartao(entity);

            await _clienteWriteRepository.UpdateAsync(clienteEntity);

            await CommitAsync(serviceResult);

            return serviceResult;
        }
    }
}
