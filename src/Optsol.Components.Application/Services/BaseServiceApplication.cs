using AutoMapper;
using Flunt.Notifications;
using Flunt.Validations;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Data;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Domain.Pagination;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optsol.Components.Application.Services
{
    public abstract class BaseServiceApplication : IBaseServiceApplication
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly NotificationContext _notificationContext;

        public BaseServiceApplication(IMapper mapper, ILoggerFactory logger, NotificationContext notificationContext)
        {
            _logger = logger.CreateLogger(nameof(BaseServiceApplication));
            _logger?.LogInformation($"Inicializando Application Service");

            _mapper = mapper ?? throw new AutoMapperNullException();

            _notificationContext = notificationContext ?? throw new NotificationContextException();
        }

        public virtual void Dispose() { }
    }

    public class BaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData> : BaseServiceApplication,
        IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>
        where TEntity : AggregateRoot
        where TGetByIdDto : BaseDataTransferObject
        where TGetAllDto : BaseDataTransferObject
        where TInsertData : BaseDataTransferObject
        where TUpdateData : BaseDataTransferObject

    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IReadRepository<TEntity, Guid> _readRepository;
        protected readonly IWriteRepository<TEntity, Guid> _writeRepository;

        public Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes { get; set; }

        public BaseServiceApplication(
            IMapper mapper,
            ILoggerFactory logger,
            IUnitOfWork unitOfWork,
            IReadRepository<TEntity, Guid> readRepository,
            IWriteRepository<TEntity, Guid> writeRepository,
            NotificationContext notificationContext) :
            base(mapper, logger, notificationContext)
        {
            _logger?.LogInformation($"Inicializando Application Service<{ typeof(TEntity).Name }, Guid>");

            _readRepository = readRepository;

            _writeRepository = writeRepository;

            _unitOfWork = unitOfWork ?? throw new UnitOfWorkNullException();
        }

        public virtual async Task<TGetByIdDto> GetByIdAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TGetByIdDto).Name }");

            var entity = await _readRepository.GetByIdAsync(id, Includes);

            return _mapper.Map<TGetByIdDto>(entity);
        }

        public virtual async Task<IEnumerable<TGetByIdDto>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdsAsync) }({{ id:{ ids } }}) Retorno: type { typeof(TGetByIdDto).Name }");

            var entity = await _readRepository.GetByIdsAsync(ids, Includes);

            return _mapper.Map<IEnumerable<TGetByIdDto>>(entity);
        }

        public virtual async Task<IEnumerable<TGetAllDto>> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TGetAllDto).Name }>");

            var entities = await _readRepository.GetAllAsync(Includes);

            return _mapper.Map<IEnumerable<TGetAllDto>>(entities);
        }

        public virtual async Task<SearchResult<TGetAllDto>> GetAllAsync<TSearch>(ISearchRequest<TSearch> requestSearch)
            where TSearch : class
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TGetAllDto).Name }>");

            var entities = await _readRepository.GetAllAsync(requestSearch);

            return _mapper.Map<SearchResult<TGetAllDto>>(entities);
        }

        public virtual async Task<TCustom> InsertAsync<TCustom>(TInsertData data)
            where TCustom: BaseDataTransferObject
        {
            data.Validate();
            if (CheckInvalidFromNotifiable(data))
            {
                return default;
            }

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);
            _logger?.LogInformation($"Método: { nameof(InsertAsync) } Mapper: { typeof(TInsertData).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            entity.Validate();
            if (CheckInvalidFromNotifiable(entity))
            {
                return default;
            }

            await _writeRepository.InsertAsync(entity);
            await CommitAsync();

            return _mapper.Map<TCustom>(entity);
        }

        public virtual Task<BaseDataTransferObject> InsertAsync(TInsertData data)
        {
            return InsertAsync<BaseDataTransferObject>(data);
        }

        public virtual async Task<TCustom> UpdateAsync<TCustom>(TUpdateData data)
            where TCustom : BaseDataTransferObject
        {
            data.Validate();
            if (CheckInvalidFromNotifiable(data))
            {
                return default;
            }

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) } Mapper: { typeof(TUpdateData).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            var entityNotFound = (await _readRepository.GetByIdAsync(entity.Id)) == null;
            if (entityNotFound)
            {
                _notificationContext.AddNotification(entity.Id.ToString(), "Registro não foi encontrado.");
            }

            entity.Validate();
            if (CheckInvalidFromNotifiable(entity))
            {
                return default;
            }

            await _writeRepository.UpdateAsync(entity);
            await CommitAsync();

            return _mapper.Map<TCustom>(entity);
        }

        public virtual Task<BaseDataTransferObject> UpdateAsync(TUpdateData data)
        {
            return UpdateAsync<BaseDataTransferObject>(data);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id } }})");

            var entityNotFound = (await _readRepository.GetByIdAsync(id)) == null;
            if (entityNotFound)
            {
                _notificationContext.AddNotification(id.ToString(), "Registro não foi encontrado ou já foi removido.");
            }

            await _writeRepository.DeleteAsync(id);
            await CommitAsync();
        }

        public virtual async Task<bool> CommitAsync()
        {
            if (_notificationContext.HasNotifications)
            {
                return false;
            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _notificationContext.AddNotification("Error", ex.Message);

                throw;
            }

            return true;
        }

        public virtual bool CheckInvalidFromNotifiable(Notifiable<Notification> data)
        {
            if (data.Notifications.Count == 0) return false;

            _notificationContext.AddNotifications(data.Notifications);

            _logger?.LogInformation($"Método: { nameof(CheckInvalidFromNotifiable) } Invalid: { _notificationContext.HasNotifications } Notifications: { _notificationContext.Notifications.ToJson() }");

            return true;
        }

        public override void Dispose()
        {

            GC.SuppressFinalize(this);
            _unitOfWork.Dispose();
        }
    }
}
