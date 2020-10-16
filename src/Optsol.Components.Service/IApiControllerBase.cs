using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Optsol.Components.Application.ViewModel;
using Optsol.Components.Domain;

namespace Optsol.Components.Service
{
    public interface IApiControllerBase<TEntity, TKey>
        where TEntity: IAggregateRoot<TKey>
    {
        Task<IActionResult> GetAllAsync<TViewModel>() where TViewModel: BaseViewModel;
        Task<IActionResult> GetByIdAsync<TViewModel>(TKey id) where TViewModel: BaseViewModel;
        Task<IActionResult> InsertAsync<TViewModel>(TViewModel viewModel) where TViewModel: BaseViewModel;
        Task<IActionResult> UpdateAsync<TViewModel>(TViewModel viewModel) where TViewModel: BaseViewModel;
        Task<IActionResult> DeleteAsync(TKey id);
    }
}