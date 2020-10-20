using System;
using System.Threading.Tasks;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.ViewModel;
using Optsol.Components.Domain;

namespace Optsol.Components.Application.Service
{
    public interface IBaseServiceApplication<TEntity, TKey>: IDisposable
        where TEntity: IAggregateRoot<TKey>
    {        
        Task<ServiceResult<TViewModel>> GetByIdAsync<TViewModel>(TKey id) where TViewModel: BaseViewModel;
        Task<ServiceResultList<TViewModel>> GetAllAsync<TViewModel>() where TViewModel: BaseViewModel;
        Task<ServiceResult> InsertAsync<TViewModel>(TViewModel viewModel) where TViewModel: BaseViewModel;  
        Task<ServiceResult> UpdateAsync<TViewModel>(TViewModel viewModel) where TViewModel: BaseViewModel;
        Task<ServiceResult> DeleteAsync(TKey id);
    }
}