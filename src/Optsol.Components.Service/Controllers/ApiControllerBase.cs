using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Service.Filters;
using Optsol.Components.Service.Responses;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;
using System;
using System.Threading.Tasks;

namespace Optsol.Components.Service.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        public IActionResult CreateResult(Response response)
        {
            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        public IActionResult CreateResult<TData>(Response<TData> response)
            where TData : BaseDataTransferObject
        {
            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        public IActionResult CreateResult<TData>(ResponseList<TData> response)
            where TData : BaseDataTransferObject
        {
            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(ValidationModelAttribute))]
    public class ApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>
        : ApiControllerBase, IApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData>
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
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }})");

            var viewModelOfResultService = await _serviceApplication.GetByIdAsync(id);

            return CreateResult(_responseFactory.Create(viewModelOfResultService));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IActionResult");

            var viewModelsOfResultService = await _serviceApplication.GetAllAsync();

            return CreateResult(_responseFactory.Create(viewModelsOfResultService));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> InsertAsync([FromBody] TInsertData data)
        {
            if (data == null)
                return NoContent();

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ data.ToJson() } }})");

            await _serviceApplication.InsertAsync(data);

            return CreateResult(_responseFactory.Create());
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateAsync([FromBody] TUpdateData data)
        {
            if (data == null)
                return NoContent();

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ data.ToJson() } }})");

            await _serviceApplication.UpdateAsync(data);

            return CreateResult(_responseFactory.Create());

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id } }})");

            await _serviceApplication.DeleteAsync(id);

            return CreateResult(_responseFactory.Create());
        }

        
    }
}
