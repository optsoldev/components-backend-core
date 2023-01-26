using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Domain.Data;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Domain.Pagination;
using Optsol.Components.Infra.Data.Pagination;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Optsol.Components.Domain.Repositories;

namespace Optsol.Components.Application.Services
{
    public abstract class BaseServiceApplication : IBaseServiceApplication
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly NotificationContext _notificationContext;

        protected BaseServiceApplication(
            IMapper mapper, 
            ILoggerFactory logger, 
            NotificationContext notificationContext)
        {
            _logger = logger.CreateLogger(nameof(BaseServiceApplication));
            _logger?.LogInformation($"Inicializando Application Service");

            _mapper = mapper ?? throw new AutoMapperNullException();

            _notificationContext = notificationContext ?? throw new NotificationContextException();
        }
    }

    public class BaseServiceApplication<TEntity> : BaseServiceApplication, IDisposable, IBaseServiceApplication<TEntity>
        where TEntity : AggregateRoot
    {
        private bool disposed = false;

        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IReadRepository<TEntity, Guid> _readRepository;
        protected readonly IWriteRepository<TEntity, Guid> _writeRepository;
        protected readonly IValidationService _validationService;
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes { get; set; }

        public BaseServiceApplication(
            IMapper mapper,
            ILoggerFactory logger,
            IUnitOfWork unitOfWork,
            IReadRepository<TEntity, Guid> readRepository,
            IWriteRepository<TEntity, Guid> writeRepository,
            NotificationContext notificationContext,
            IValidationService validationService = null) :
            base(mapper, logger, notificationContext)
        {
            _logger?.LogInformation($"Inicializando Application Service<{ typeof(TEntity).Name }, Guid>");

            _readRepository = readRepository;

            _writeRepository = writeRepository;

            _unitOfWork = unitOfWork ?? throw new UnitOfWorkNullException();

            _validationService = validationService;
        }

        public virtual async Task<TResponse> GetByIdAsync<TResponse>(Guid id)
            where TResponse : BaseModel
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TResponse).Name }");

            var entity = await _readRepository.GetByIdAsync(id, Includes);

            return _mapper.Map<TResponse>(entity);
        }

        public virtual async Task<IEnumerable<TResponse>> GetByIdsAsync<TResponse>(IEnumerable<Guid> ids)
            where TResponse : BaseModel
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdsAsync) }({{ id:{ ids } }}) Retorno: type { typeof(TResponse).Name }");

            var entity = await _readRepository.GetByIdsAsync(ids, Includes);

            return _mapper.Map<IEnumerable<TResponse>>(entity);
        }

        public virtual async Task<IEnumerable<TResponse>> GetAllAsync<TResponse>()
            where TResponse : BaseModel
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TResponse).Name }>");

            var entities = await _readRepository.GetAllAsync(Includes);

            return _mapper.Map<IEnumerable<TResponse>>(entities);
        }

        public virtual async Task<ISearchResult<TResponse>> GetAllAsync<TResponse, TSearch>(ISearchRequest<TSearch> requestSearch)
            where TSearch : class
            where TResponse : BaseModel
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TResponse).Name }>");

            var entities = await _readRepository.GetAllAsync(requestSearch);

            return _mapper.Map<SearchResult<TResponse>>(entities);
        }

        public async virtual Task<TResponse> InsertAsync<TRequest, TResponse>(TRequest data)
            where TRequest : BaseModel
            where TResponse : BaseModel
        {
            data.Validate();
            if (CheckInvalidFromNotifiable(data))
            {
                return default;
            }

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);
            _logger?.LogInformation($"Método: { nameof(InsertAsync) } Mapper: { typeof(TRequest).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            entity.Validate();
            if (CheckInvalidFromNotifiable(entity))
            {
                return default;
            }

            if (_validationService is not null)
            {
                _validationService.SetEntity(entity);
                _validationService.SetRequestModel(data);
                _validationService.InsertValidation();
            
                if (_notificationContext.HasNotifications)
                {
                    return default;
                }
            }
            
            await _writeRepository.InsertAsync(entity);
            await CommitAsync();

            return _mapper.Map<TResponse>(entity);
        }

        public async virtual Task<TResponse> UpdateAsync<TRequest, TResponse>(Guid id, TRequest data)
            where TRequest : BaseModel
            where TResponse : BaseModel
        {
            data.Validate();
            if (CheckInvalidFromNotifiable(data))
            {
                return default;
            }

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) } Mapper: { typeof(TRequest).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            var updateEntity = await _readRepository.GetByIdAsync(id);
            var dataForUpdateNotFound = updateEntity == null;
            if (dataForUpdateNotFound)
            {
                _notificationContext.AddNotification(id.ToString(), "Registro não foi encontrado.");
            }

            ForceId(entity, id);
            entity.Validate();

            if (CheckInvalidFromNotifiable(entity))
            {
                return default;
            }

            if (_validationService is not null)
            {
                _validationService.SetEntity(entity);
                _validationService.SetRequestModel(data);
                _validationService.UpdateValidation();
            
                if (_notificationContext.HasNotifications)
                {
                    return default;
                }
            }

            await _writeRepository.UpdateAsync(entity);
            await CommitAsync();

            return _mapper.Map<TResponse>(entity);
        }

        private static void ForceId(AggregateRoot aggregateRoot, Guid idDatabase)
        {
            aggregateRoot.GetType().GetProperty(nameof(aggregateRoot.Id)).SetValue(aggregateRoot, idDatabase, null);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _logger?.LogInformation($"Método: { nameof(Dispose) }()");

            if (!disposed && disposing)
            {
                _unitOfWork.Dispose();
            }

            disposed = true;
        }
    }
}
