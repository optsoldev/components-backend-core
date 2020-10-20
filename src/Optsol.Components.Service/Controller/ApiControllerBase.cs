using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Application.ViewModel;
using Optsol.Components.Domain;
using Optsol.Components.Service.Response;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;

namespace Optsol.Components.Service
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase<TEntity, TKey> : ControllerBase, IApiControllerBase<TEntity, TKey>
        where TEntity: IAggregateRoot<TKey>
    {
        protected readonly ILogger<ApiControllerBase<TEntity, TKey>> _logger;
        protected readonly IBaseServiceApplication<TEntity, TKey> _applicationService;
        protected readonly IResponseFactory _responseFactory;

        public ApiControllerBase(ILogger<ApiControllerBase<TEntity, TKey>> logger, IResponseFactory responseFactory, IBaseServiceApplication<TEntity, TKey> applicationService)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Controller Base<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");

            _responseFactory = responseFactory ?? throw new ResponseFactoryNullException();
            _applicationService = applicationService;
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetByIdAsync<TViewModel>(TKey id)
            where TViewModel: BaseViewModel
        {
            if(id == null)
                return NotFound();
            
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TViewModel).Name }");

            var serviceResult = await _applicationService.GetByIdAsync<TViewModel>(id);

            return Ok(_responseFactory.Create(serviceResult));
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync<TViewModel>()
            where TViewModel: BaseViewModel
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TViewModel).Name }>");

            var serviceResult = await _applicationService.GetAllAsync<TViewModel>();

            return Ok(_responseFactory.Create(serviceResult));
        }

        [HttpPost("{viewModel}")]
        public virtual async Task<IActionResult> InsertAsync<TViewModel>(TViewModel viewModel)
            where TViewModel: BaseViewModel
        {
            if(viewModel == null)
                return NotFound();
            
            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ viewModel.ToJson() } }})");

            var serviceResult = await _applicationService.InsertAsync<TViewModel>(viewModel);

            return Ok(_responseFactory.Create(serviceResult));
        }

        public virtual async Task<IActionResult> UpdateAsync<TViewModel>(TViewModel viewModel)
            where TViewModel: BaseViewModel
        {
            if(viewModel == null)
                return NotFound();
            
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ viewModel.ToJson() } }})");

            var serviceResult = await _applicationService.UpdateAsync<TViewModel>(viewModel);

            return Ok(_responseFactory.Create(serviceResult));
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id } }})");

            var serviceResult = await _applicationService.DeleteAsync(id);

            return Ok(_responseFactory.Create(serviceResult));
        }
    }
}
