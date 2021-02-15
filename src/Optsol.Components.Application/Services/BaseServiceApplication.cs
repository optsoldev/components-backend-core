using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Notifications;
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

        public BaseServiceApplication(IMapper mapper, ILogger logger, NotificationContext notificationContext)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Application Service");

            _mapper = mapper ?? throw new AutoMapperNullException();

            _notificationContext = notificationContext ?? throw new NotificationContextException();
        }

        public abstract void Dispose();
    }

    public class BaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>
        : BaseServiceApplication, IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>
        where TEntity : AggregateRoot
        where TGetByIdDto : BaseDataTransferObject
        where TGetAllDto : BaseDataTransferObject
        where TInsertData : BaseDataTransferObject
        where TUpdateData : BaseDataTransferObject
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IReadRepository<TEntity, Guid> _readRepository;
        protected readonly IWriteRepository<TEntity, Guid> _writeRepository;

        public BaseServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>> logger,
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
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TGetAllDto).Name }");

            var entity = await _readRepository.GetByIdAsync(id);

            return _mapper.Map<TGetByIdDto>(entity);
        }

        public virtual async Task<IEnumerable<TGetAllDto>> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TGetAllDto).Name }>");

            var entities = await _readRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<TGetAllDto>>(entities);
        }

        public virtual async Task<SearchResult<TGetAllDto>> GetAllAsync<TSearch>(RequestSearch<TSearch> requestSearch)
            where TSearch : class
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TGetAllDto).Name }>");

            var entities = await _readRepository.GetAllAsync(requestSearch);

            return _mapper.Map<SearchResult<TGetAllDto>>(entities);
        }

        public virtual async Task InsertAsync(TInsertData data)
        {
            data.Validate();
            if (data.Invalid)
            {
                _notificationContext.AddNotifications(data.Notifications);

                LogNotifications(nameof(InsertAsync));

                return;
            }

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);

            _logger?.LogInformation($"Método: { nameof(InsertAsync) } Mapper: { typeof(TInsertData).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            entity.Validate();

            _notificationContext.AddNotifications((entity as Entity<Guid>).Notifications);

            LogNotifications(nameof(InsertAsync));

            await _writeRepository.InsertAsync(entity);

            await CommitAsync();
            
        }

        public virtual async Task UpdateAsync(TUpdateData data)
        {
            data.Validate();
            if (data.Invalid)
            {
                _notificationContext.AddNotifications(data.Notifications);

                LogNotifications(nameof(UpdateAsync));

                return;
            }

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) } Mapper: { typeof(TUpdateData).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            var entitySelectForUpdate = await _readRepository.GetByIdAsync(entity.Id);

            var entityNotFound = entitySelectForUpdate == null;
            if (entityNotFound)
            {
                _notificationContext.AddNotification(entity.Id.ToString(), "Registro não foi encontrado.");
            }

            entity.Validate();
            _notificationContext.AddNotifications(entity.Notifications);

            var hasNotifications = _notificationContext.HasNotifications;
            if (hasNotifications)
            {
                LogNotifications(nameof(entity));
            }

            await _writeRepository.UpdateAsync(entity);

            await CommitAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id.ToString() } }})");

            await _writeRepository.DeleteAsync(id);

            await CommitAsync();
        }

        public virtual async Task<bool> CommitAsync()
        {
            if (_notificationContext.HasNotifications) return false;
            if ((await _unitOfWork.CommitAsync())) return true;

            _notificationContext.AddNotification("Commit", "Houve um problema ao salvar os dados!");
            return false;
        }

        private void LogNotifications(string method)
        {
            if (_notificationContext.HasNotifications)
                _logger?.LogInformation($"Método: { method } Invalid: { _notificationContext.HasNotifications } Notifications: { _notificationContext.Notifications.ToJson() }");
        }

        public override void Dispose()
        {

            GC.SuppressFinalize(this);
            _unitOfWork.Dispose();
        }
    }
}
