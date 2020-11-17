using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;
using Optsol.Playground.Infra.Data.Repositories.Cliente;
namespace Optsol.Playground.Application.Services.Cliente
{
    public class ClienteServiceApplication : BaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>,
        IClienteServiceApplication
    {
        protected readonly IClienteReadRepository _clienteReadRepository;

        public ClienteServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, InsertClienteViewModel, UpdateClienteViewModel>> logger,
            IServiceResultFactory serviceResultFactory,
            IUnitOfWork unitOfWork,
            IReadRepository<ClienteEntity, Guid> readRepository,
            IWriteRepository<ClienteEntity, Guid> writeRepository,
            IClienteReadRepository clienteReadRepository)
            : base(mapper, logger, serviceResultFactory, unitOfWork, readRepository, writeRepository)
        {
            _clienteReadRepository = clienteReadRepository;
        }

        public async Task<ServiceResult<ClienteComCartoesViewModel>> GetClienteComCartaoCredito(Guid id)
        {
            var clienteEntity = await _clienteReadRepository.GetClienteComCartaoCredito(id);
            var clienteViewModel = _mapper.Map<ClienteComCartoesViewModel>(clienteEntity);

            return _serviceResultFactory.Create(clienteViewModel);
        }
    }
}
