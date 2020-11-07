using System;
using AutoMapper;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data;
using Optsol.Components.Shared.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.DataTransferObject;

namespace Optsol.Components.Application.Service
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

        public BaseServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>> logger,
            IServiceResultFactory serviceResultFactory,
            IUnitOfWork unitOfWork,
            IReadRepository<TEntity, Guid> readRepository,
            IWriteRepository<TEntity, Guid> writeRepository)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Application Service<{ typeof(TEntity).Name }, Guid>");

            _serviceResultFactory = serviceResultFactory ?? throw new ServiceResultNullException();
            _unitOfWork = unitOfWork ?? throw new UnitOfWorkNullException();
            _mapper = mapper ?? throw new AutoMapperNullException();
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public async Task<ServiceResult<TGetByIdDto>> GetByIdAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TGetAllDto).Name }");

            var entity = await _readRepository.GetByIdAsync(id);

            return _serviceResultFactory.Create(_mapper.Map<TGetByIdDto>(entity));
        }

        public async Task<ServiceResultList<TGetAllDto>> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TGetAllDto).Name }>");

            var entities = await _readRepository.GetAllAsync().AsyncEnumerableToEnumerable();

            return _serviceResultFactory.Create(entities.Select(entity => _mapper.Map<TGetAllDto>(entity)));
        }

        public async Task<ServiceResult> InsertAsync(TInsertData data)
        {
            var serviceResult = _serviceResultFactory.Create();

            data.Validate();
            if (data.Invalid)
            {
                serviceResult.AddNotifications(data);
                LogNotifications(nameof(InsertAsync), serviceResult);
                return serviceResult;
            }

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);

            _logger?.LogInformation($"Método: { nameof(InsertAsync) } Mapper: { typeof(TInsertData).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            entity.Validate();
            serviceResult.AddNotifications((entity as Entity<Guid>));
            LogNotifications(nameof(InsertAsync), serviceResult);

            await _writeRepository.InsertAsync(entity);

            await CommitAsync(serviceResult);

            return serviceResult;
        }

        public async Task<ServiceResult> UpdateAsync(TUpdateData data)
        {
            var serviceResult = _serviceResultFactory.Create();

            data.Validate();
            if (data.Invalid)
            {
                serviceResult.AddNotifications(data);
                LogNotifications(nameof(UpdateAsync), serviceResult);
                return serviceResult;
            }

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ data.ToJson() } }})");

            var entity = _mapper.Map<TEntity>(data);
            var edit = await _readRepository.GetByIdAsync(entity.Id);

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) } Mapper: { typeof(TUpdateData).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            entity.Validate();
            serviceResult.AddNotifications((entity as Entity<Guid>));
            LogNotifications(nameof(entity), serviceResult);

            await _writeRepository.UpdateAsync(entity);

            await CommitAsync(serviceResult);

            return serviceResult;
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var serviceResult = _serviceResultFactory.Create();

            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id.ToString() } }})");

            await _writeRepository.DeleteAsync(id);

            await CommitAsync(serviceResult);

            return serviceResult;
        }

        public virtual async Task<Boolean> CommitAsync(ServiceResult serviceResult)
        {
            if (serviceResult.Invalid) return false;
            if ((await _unitOfWork.CommitAsync())) return true;

            serviceResult.AddNotification("Commit", "Houve um problema ao salvar os dados!");
            return false;
        }

        public void Dispose()
        {

            GC.SuppressFinalize(this);
            _unitOfWork.Dispose();
        }

        private void LogNotifications(string method, ServiceResult serviceResult)
        {
            if (serviceResult.Invalid)
                _logger?.LogInformation($"Método: { method } Valid: { serviceResult.Valid } Notifications: { serviceResult.Notifications.ToJson() }");
        }
    }
}
