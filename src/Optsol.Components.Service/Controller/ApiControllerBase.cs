using System;
using System.Threading.Tasks;
using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Application.ViewModel;
using Optsol.Components.Domain;
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

        public ApiControllerBase(ILogger<ApiControllerBase<TEntity, TKey>> logger, IBaseServiceApplication<TEntity, TKey> applicationService)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Controller Base<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");

            _applicationService = applicationService;
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetByIdAsync<TViewModel>(TKey id)
            where TViewModel: BaseViewModel
        {
            if(id == null)
                return NotFound();
            
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TViewModel).Name }");

            return Ok((await _applicationService.GetByIdAsync<TViewModel>(id)));
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync<TViewModel>()
            where TViewModel: BaseViewModel
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TViewModel).Name }>");

            return Ok((await _applicationService.GetAllAsync<TViewModel>()));
        }

        [HttpPost("{viewModel}")]
        public virtual async Task<IActionResult> InsertAsync<TViewModel>(TViewModel viewModel)
            where TViewModel: BaseViewModel
        {
            if(viewModel == null)
                return NotFound();
            
            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ viewModel.ToJson() } }})");

            await _applicationService.InsertAsync<TViewModel>(viewModel);

            return Ok();
        }

        public virtual async Task<IActionResult> UpdateAsync<TViewModel>(TViewModel viewModel)
            where TViewModel: BaseViewModel
        {
            if(viewModel == null)
                return NotFound();
            
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ viewModel.ToJson() } }})");

            await _applicationService.UpdateAsync<TViewModel>(viewModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id } }})");

            await _applicationService.DeleteAsync(id);
                      
            return Ok();
        }
    }
}
