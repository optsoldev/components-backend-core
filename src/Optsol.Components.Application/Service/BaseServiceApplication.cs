using System;
using AutoMapper;
using Optsol.Components.Domain;
using Optsol.Components.Infra.Data;
using Optsol.Components.Shared.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Application.ViewModel;
using Optsol.Components.Application.Result;

namespace Optsol.Components.Application.Service
{
    public class BaseServiceApplication<TEntity, TKey> 
        : IBaseServiceApplication<TEntity, TKey>, IDisposable
        where TEntity: class, IAggregateRoot<TKey>
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IReadRepository<TEntity, TKey> _readRepository;
        protected readonly IWriteRepository<TEntity, TKey> _writeRepository;
        protected readonly IServiceResultFactory _serviceResultFactory;

        public BaseServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<TEntity, TKey>> logger,
            IServiceResultFactory serviceResultFactory,
            IUnitOfWork unitOfWork,
            IReadRepository<TEntity, TKey> readRepository, 
            IWriteRepository<TEntity, TKey> writeRepository)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Application Service<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");
            
            _serviceResultFactory = serviceResultFactory ?? throw new ServiceResultNullException();
            _unitOfWork = unitOfWork ?? throw new UnitOfWorkNullException();
            _mapper = mapper ?? throw new AutoMapperNullException();
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public virtual async Task<ServiceResut<TViewModel>> GetByIdAsync<TViewModel>(TKey id)
            where TViewModel: BaseViewModel
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TViewModel).Name }");
            
            var entity = await _readRepository.GetByIdAsync(id);
            
            return _serviceResultFactory.Create(_mapper.Map<TViewModel>(entity));
        }

        public virtual async Task<ServiceResultList<TViewModel>> GetAllAsync<TViewModel>()
            where TViewModel: BaseViewModel
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TViewModel).Name }>");
            
            var entities = await _readRepository.GetAllAsync().AsyncEnumerableToEnumerable();

            return _serviceResultFactory.Create(entities.Select(entity => _mapper.Map<TViewModel>(entity)));
        }

        public virtual async Task<ServiceResult> InsertAsync<TViewModel>(TViewModel viewModel)
            where TViewModel: BaseViewModel
        {
            var serviceResult =_serviceResultFactory.Create();

            viewModel.Validate();
            if(viewModel.Invalid)
            {
                serviceResult.AddNotifications(viewModel);
                LogNotifications(nameof(InsertAsync), serviceResult);
                return serviceResult;
            }

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ viewModel.ToJson() } }})");
            
            var entity = _mapper.Map<TEntity>(viewModel);

            _logger?.LogInformation($"Método: { nameof(InsertAsync) } Mapper: { typeof(TViewModel).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");
            
            entity.Validate();
            serviceResult.AddNotifications((entity as Entity<TKey>));
            LogNotifications(nameof(InsertAsync), serviceResult);
            
            await _writeRepository.InsertAsync(entity);

            await CommitAsync(serviceResult);

            return serviceResult;
        }

        
        public virtual async Task<ServiceResult> UpdateAsync<TViewModel>(TViewModel viewModel)
            where TViewModel: BaseViewModel
        {
            var serviceResult =_serviceResultFactory.Create();

            viewModel.Validate();
            if(viewModel.Invalid)
            {
                serviceResult.AddNotifications(viewModel);
                LogNotifications(nameof(UpdateAsync), serviceResult);
                return serviceResult;
            }

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ viewModel.ToJson() } }})");
            
            var entity = _mapper.Map<TEntity>(viewModel);
            var edit = await _readRepository.GetByIdAsync(entity.Id);
                         
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) } Mapper: { typeof(TViewModel).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            entity.Validate();
            serviceResult.AddNotifications((entity as Entity<TKey>));
            LogNotifications(nameof(entity), serviceResult);

            await _writeRepository.UpdateAsync(entity);

            await CommitAsync(serviceResult);

            return serviceResult;
        }

        public virtual async Task<ServiceResult> DeleteAsync(TKey id)
        {
            var serviceResult =_serviceResultFactory.Create();

            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id.ToString() } }})");

            await _writeRepository.DeleteAsync(id);

            await CommitAsync(serviceResult);

            return serviceResult;
        }

        public virtual async Task<Boolean> CommitAsync(ServiceResult serviceResult)
        {
            if(serviceResult.Invalid) return false;
            if((await _unitOfWork.CommitAsync())) return true;

            serviceResult.AddNotification("Commit", "Houve um problema ao salvar os dados!");
            return false;
        }

        public void Dispose()
        {
            _writeRepository.Dispose();
            _readRepository.Dispose();
        }

        private void LogNotifications(string method, ServiceResult serviceResult)
        {
            if(serviceResult.Invalid)
                _logger?.LogInformation($"Método: { method } Valid: { serviceResult.Valid } Notifications: { serviceResult.Notifications.ToJson() }");
        }

    }
}
