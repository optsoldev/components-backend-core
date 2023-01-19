using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Pagination;
using Optsol.Components.Infra.Data.Pagination;
using Optsol.Components.Service.Responses;
using Optsol.Components.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Components.Application;

namespace Optsol.Components.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase, IApiControllerBase
    {
        protected readonly IResponseFactory _responseFactory;

        public ApiControllerBase(IResponseFactory responseFactory)
        {
            _responseFactory = responseFactory ?? throw new ResponseFactoryNullException();
        }

        protected IActionResult CreateResult()
        {
            var response = _responseFactory.Create();

            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        protected IActionResult CreateResult<TData>(TData viewModelOfResultService)
            where TData : BaseModel
        {
            var response = _responseFactory.Create(viewModelOfResultService);
            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        protected IActionResult CreateResult<TData>(IEnumerable<TData> viewModelsOfResultService)
            where TData : BaseModel
        {
            var response = _responseFactory.Create(viewModelsOfResultService);

            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        protected IActionResult CreateResult<TData>(ISearchResult<TData> viewModelsOfResultService)
            where TData : BaseModel
        {
            var response = _responseFactory.Create(viewModelsOfResultService);

            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }
    }

    public class ApiControllerBase<TEntity, TRequest, TResponse> : ApiControllerBase,
        IApiControllerBase<TEntity, TRequest, TResponse>
        where TEntity : AggregateRoot
        where TRequest : BaseModel
        where TResponse : BaseModel
    {
        protected readonly ILogger _logger;
        protected readonly IBaseServiceApplication<TEntity> _serviceApplication;

        public ApiControllerBase(
            ILoggerFactory logger,
            IBaseServiceApplication<TEntity> serviceApplication,
            IResponseFactory responseFactory) : base(responseFactory)
        {
            _logger = logger.CreateLogger(nameof(ApiControllerBase));
            _logger?.LogInformation($"Inicializando Controller Base<{ typeof(TEntity).Name }, Guid>");

            _serviceApplication = serviceApplication;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetByIdAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }})");

            var viewModelOfResultService = await _serviceApplication.GetByIdAsync<TResponse>(id);

            return CreateResult(viewModelOfResultService);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IActionResult");

            var viewModelsOfResultService = await _serviceApplication.GetAllAsync<TResponse>();

            return CreateResult(viewModelsOfResultService);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public virtual async Task<IActionResult> InsertAsync([FromBody] TRequest data)
        {
            if (data == null)
                return NoContent();

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ data.ToJson() } }})");

            return CreateResult(await _serviceApplication.InsertAsync<TRequest, TResponse>(data));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public virtual async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] TRequest data)
        {
            if (data == null)
                return NoContent();

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ data.ToJson() } }})");

            return CreateResult(await _serviceApplication.UpdateAsync<TRequest, TResponse>(id, data));

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id } }})");

            await _serviceApplication.DeleteAsync(id);

            return CreateResult();
        }
    }

    public class ApiControllerBase<TEntity, TRequest, TResponse, TSearch> :
            ApiControllerBase<TEntity, TRequest, TResponse>,
            IApiControllerBase<TEntity, TRequest, TResponse, TSearch>
            where TEntity : AggregateRoot
            where TRequest : BaseModel
            where TResponse : BaseModel
            where TSearch : class
    {
        public ApiControllerBase(
            ILoggerFactory logger,
            IBaseServiceApplication<TEntity> serviceApplication,
            IResponseFactory responseFactory) : base(logger, serviceApplication, responseFactory)
        {

        }

        [HttpPost("paginated")]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetAllAsync(SearchRequest<TSearch> search)
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }({ search.ToJson() }) Retorno: IActionResult");

            var viewModelsOfResultService = await _serviceApplication.GetAllAsync<TResponse, TSearch>(search);

            return CreateResult(viewModelsOfResultService);
        }
    }
}