using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.DataTransferObject;
using Optsol.Components.Application.Service;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Service.Filters;
using Optsol.Components.Service.Response;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;

namespace Optsol.Components.Service
{
    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(ValidationModelAttribute))]
    public class ApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>
        : ControllerBase, IApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>
        where TEntity : AggregateRoot
        where TGetByIdDto : BaseDataTransferObject
        where TGetAllDto : BaseDataTransferObject
        where TInsertData : BaseDataTransferObject
        where TUpdateData : BaseDataTransferObject
    {
        protected readonly ILogger<ApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>> _logger;
        protected readonly IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData> _serviceApplication;
        protected readonly IResponseFactory _responseFactory;

        public ApiControllerBase(
            ILogger<ApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>> logger,
            IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData> serviceApplication,
            IResponseFactory responseFactory)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Controller Base<{ typeof(TEntity).Name }, Guid>");

            _responseFactory = responseFactory ?? throw new ResponseFactoryNullException();
            _serviceApplication = serviceApplication;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }})");

            var serviceResult = await _serviceApplication.GetByIdAsync(id);

            return Ok(_responseFactory.Create(serviceResult));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IActionResult");

            var serviceResult = await _serviceApplication.GetAllAsync();

            return Ok(_responseFactory.Create(serviceResult));
        }

        [HttpPost]
        public async Task<IActionResult> InsertAsync(TInsertData data)
        {
            if (data == null)
                return NoContent();

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ data.ToJson() } }})");

            var serviceResult = await _serviceApplication.InsertAsync(data);

            return Ok(_responseFactory.Create(serviceResult));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(TUpdateData data)
        {
            if (data == null)
                return NoContent();

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ data.ToJson() } }})");

            var serviceResult = await _serviceApplication.UpdateAsync(data);

            return Ok(_responseFactory.Create(serviceResult));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id } }})");

            var serviceResult = await _serviceApplication.DeleteAsync(id);

            return Ok(_responseFactory.Create(serviceResult));
        }
    }
}
