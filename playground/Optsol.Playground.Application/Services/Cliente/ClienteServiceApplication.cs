using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;
using Optsol.Components.Infra.UoW;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;
using Optsol.Playground.Domain.Repositories.Cliente;
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
            IClienteReadRepository clienteReadRepository)
            : base(mapper, logger, serviceResultFactory, unitOfWork, clienteReadRepository, clienteWriteRepository)
        {
            _clienteReadRepository = clienteReadRepository;
            _clienteWriteRepository = clienteWriteRepository;
        }

        public async Task<ServiceResult<ClienteComCartoesViewModel>> GetClienteComCartaoCreditoAsync(Guid id)
        {
            var clienteEntity = await _clienteReadRepository.GetClienteComCartaoCreditoAsync(id);
            var clienteViewModel = _mapper.Map<ClienteComCartoesViewModel>(clienteEntity);

            return _serviceResultFactory.Create(clienteViewModel);
        }

        public async Task<ServiceResult> InserirCartaoNoClienteAsync(InsertCartaoCreditoViewModel insertCartaoCreditoViewModel)
        {
            var serviceResult = _serviceResultFactory.Create();

            var clienteEntity = await _clienteReadRepository.GetByIdAsync(insertCartaoCreditoViewModel.ClienteId);

            var entity = _mapper.Map<CartaoCreditoEntity>(insertCartaoCreditoViewModel);
            clienteEntity.AdicionarCartao(entity);
            
            await _clienteWriteRepository.UpdateAsync(clienteEntity, 
                (context, entity) => 
                {
                    var dbSet = context.Set<CartaoCreditoEntity>();
                    foreach(var cartao in entity.Cartoes) 
                    {
                        var localEntity = dbSet.Local?.Where(w => w.Id.Equals(cartao.Id)).FirstOrDefault();
                        var inLocal = localEntity != null;
                        if(inLocal)
                        {
                            context.Entry(localEntity).State = EntityState.Added;
                        }
                        else
                        {
                            context.Entry(cartao).State = EntityState.Modified;
                        }
                    }
                });
            
            await CommitAsync(serviceResult);

            return serviceResult;
        }
    }
}
