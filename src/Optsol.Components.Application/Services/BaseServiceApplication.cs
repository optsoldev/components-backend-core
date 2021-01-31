using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Application.Results;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Optsol.Components.Application.Services
{
    public class BaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>
        : IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>, IDisposable
        where TEntity : AggregateRoot
        where TGetByIdDto : BaseDataTransferObject
        where TGetAllDto : BaseDataTransferObject
        where TInsertData : BaseDataTransferObject
        where TUpdateData : BaseDataTransferObject
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IReadRepository<TEntity, Guid> _readRepository;
        protected readonly IWriteRepository<TEntity, Guid> _writeRepository;
        protected readonly IServiceResultFactory _serviceResultFactory;
        protected readonly NotificationContext _notificationContext;

        public BaseServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>> logger,
            IServiceResultFactory serviceResultFactory,
            IUnitOfWork unitOfWork,
            IReadRepository<TEntity, Guid> readRepository,
            IWriteRepository<TEntity, Guid> writeRepository,
            NotificationContext notificationContext)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Application Service<{ typeof(TEntity).Name }, Guid>");

            _serviceResultFactory = serviceResultFactory ?? throw new ServiceResultNullException();
            
            _unitOfWork = unitOfWork ?? throw new UnitOfWorkNullException();
            
            _mapper = mapper ?? throw new AutoMapperNullException();

            _notificationContext = notificationContext ?? throw new NotificationContextException();

            _readRepository = readRepository;

            _writeRepository = writeRepository;
        }

        public virtual async Task<ServiceResult<TGetByIdDto>> GetByIdAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TGetAllDto).Name }");

            var entity = await _readRepository.GetByIdAsync(id);

            return _serviceResultFactory.Create(_mapper.Map<TGetByIdDto>(entity));
        }

        public virtual async Task<ServiceResultList<TGetAllDto>> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TGetAllDto).Name }>");

            var entities = await _readRepository.GetAllAsync().AsyncEnumerableToEnumerable();

            return _serviceResultFactory.Create(entities.Select(entity => _mapper.Map<TGetAllDto>(entity)));
        }

        public virtual async Task<ServiceResult> InsertAsync(TInsertData data)
        {
            var serviceResult = _serviceResultFactory.Create();

            data.Validate();
            if (data.Invalid)
            {
                _notificationContext.AddNotifications(data.Notifications);

                LogNotifications(nameof(InsertAsync));

                return serviceResult;
            }

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);

            _logger?.LogInformation($"Método: { nameof(InsertAsync) } Mapper: { typeof(TInsertData).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            entity.Validate();

            _notificationContext.AddNotifications((entity as Entity<Guid>).Notifications);

            LogNotifications(nameof(InsertAsync));

            await _writeRepository.InsertAsync(entity);

            await CommitAsync(serviceResult);

            return serviceResult;
        }

        public virtual async Task<ServiceResult> UpdateAsync(TUpdateData data)
        {
            var serviceResult = _serviceResultFactory.Create();

            data.Validate();
            if (data.Invalid)
            {
                _notificationContext.AddNotifications(data.Notifications);

                LogNotifications(nameof(UpdateAsync));

                return serviceResult;
            }

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);

            var edit = await _readRepository.GetByIdAsync(entity.Id);

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) } Mapper: { typeof(TUpdateData).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            entity.Validate();

            _notificationContext.AddNotifications((entity as Entity<Guid>).Notifications);

            LogNotifications(nameof(entity));

            await _writeRepository.UpdateAsync(entity);

            await CommitAsync(serviceResult);

            return serviceResult;
        }

        public virtual async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var serviceResult = _serviceResultFactory.Create();

            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id.ToString() } }})");

            await _writeRepository.DeleteAsync(id);

            await CommitAsync(serviceResult);

            return serviceResult;
        }

        public virtual async Task<Boolean> CommitAsync(ServiceResult serviceResult)
        {
            if (_notificationContext.HasNotifications) return false;
            if ((await _unitOfWork.CommitAsync())) return true;

            _notificationContext.AddNotification("Commit", "Houve um problema ao salvar os dados!");
            return false;
        }

        public virtual void Dispose()
        {

            GC.SuppressFinalize(this);
            _unitOfWork.Dispose();
        }

        private void LogNotifications(string method)
        {
            if (_notificationContext.HasNotifications)
                _logger?.LogInformation($"Método: { method } Invalid: { _notificationContext.HasNotifications } Notifications: { _notificationContext.Notifications.ToJson() }");
        }
    }
}
