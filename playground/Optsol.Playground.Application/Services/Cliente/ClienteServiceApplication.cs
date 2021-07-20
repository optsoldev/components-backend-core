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
    public class ClienteServiceApplication : BaseServiceApplication<ClientePessoaFisicaEntity>, IClienteServiceApplication
    {
        protected readonly IClientePessoaFisicaReadRepository _clienteReadRepository;
        protected readonly IClientePessoaFisicaWriteRepository _clienteWriteRepository;

        public ClienteServiceApplication(
            IMapper mapper,
            ILoggerFactory logger,
            IUnitOfWork unitOfWork,
            IClientePessoaFisicaWriteRepository clienteWriteRepository,
            IClientePessoaFisicaReadRepository clienteReadRepository,
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

        public async Task InserirCartaoNoClienteAsync(CartaoCreditoRequest insertCartaoCreditoViewModel)
        {
            insertCartaoCreditoViewModel.Validate();
            if(CheckInvalidFromNotifiable(insertCartaoCreditoViewModel))
            {
                return;
            }

            var clienteEntity = await _clienteReadRepository.GetByIdAsync(insertCartaoCreditoViewModel.ClienteId);

            var entity = _mapper.Map<CartaoCreditoEntity>(insertCartaoCreditoViewModel);
            entity.Validate();
            if (CheckInvalidFromNotifiable(insertCartaoCreditoViewModel))
            {
                return;
            }

            clienteEntity.AdicionarCartao(entity);

            await _clienteWriteRepository.UpdateAsync(clienteEntity);

            await CommitAsync();
        }
    }
}
