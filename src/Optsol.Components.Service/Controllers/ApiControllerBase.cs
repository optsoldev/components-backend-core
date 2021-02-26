using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.Security.Attributes;
using Optsol.Components.Service.Filters;
using Optsol.Components.Service.Responses;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            where TData : BaseDataTransferObject
        {
            var response = _responseFactory.Create(viewModelOfResultService);
            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        protected IActionResult CreateResult<TData>(IEnumerable<TData> viewModelsOfResultService)
            where TData : BaseDataTransferObject
        {
            var response = _responseFactory.Create(viewModelsOfResultService);

            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        protected IActionResult CreateResult<TData>(SearchResult<TData> viewModelsOfResultService)
            where TData : BaseDataTransferObject
        {
            var response = _responseFactory.Create(viewModelsOfResultService);

            if (response.Failure)
                return BadRequest(response);

            return Ok(response);
        }
    }

    [ValidationModelAttribute]
    public class ApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData, TSearch> : ApiControllerBase,
        IApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData, TSearch>
        where TEntity : AggregateRoot
        where TGetByIdDto : BaseDataTransferObject
        where TGetAllDto : BaseDataTransferObject
        where TInsertData : BaseDataTransferObject
        where TUpdateData : BaseDataTransferObject
        where TSearch : class, ISearch<TEntity>
    {
        protected readonly ILogger<ApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData, TSearch>> _logger;
        protected readonly IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData> _serviceApplication;


        public ApiControllerBase(
            ILogger<ApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData, TSearch>> logger,
            IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData> serviceApplication,
            IResponseFactory responseFactory) : base(responseFactory)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Controller Base<{ typeof(TEntity).Name }, Guid>");

            _serviceApplication = serviceApplication;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetByIdAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }})");

            var viewModelOfResultService = await _serviceApplication.GetByIdAsync(id);

            return CreateResult(viewModelOfResultService);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IActionResult");

            var viewModelsOfResultService = await _serviceApplication.GetAllAsync();

            return CreateResult(viewModelsOfResultService);
        }

        [HttpPost("paginated")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAllAsync(RequestSearch<TSearch> search)
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }({ search.ToJson() }) Retorno: IActionResult");

            var viewModelsOfResultService = await _serviceApplication.GetAllAsync(search);

            return CreateResult(viewModelsOfResultService);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public virtual async Task<IActionResult> InsertAsync([FromBody] TInsertData data)
        {
            if (data == null)
                return NoContent();

            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ data.ToJson() } }})");

            await _serviceApplication.InsertAsync(data);

            return CreateResult();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public virtual async Task<IActionResult> UpdateAsync([FromBody] TUpdateData data)
        {
            if (data == null)
                return NoContent();

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ data.ToJson() } }})");

            await _serviceApplication.UpdateAsync(data);

            return CreateResult();

        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> DeleteAsync(Guid id)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id } }})");

            await _serviceApplication.DeleteAsync(id);

            return CreateResult();
        }
    }
}
