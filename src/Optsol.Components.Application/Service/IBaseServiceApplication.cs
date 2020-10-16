using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Components.Application.ViewModel;
using Optsol.Components.Domain;

namespace Optsol.Components.Application.Service
{
    public interface IBaseServiceApplication<TEntity, TKey>: IDisposable
        where TEntity: IAggregateRoot<TKey>
    {        
        Task<TViewModel> GetByIdAsync<TViewModel>(TKey id) where TViewModel: BaseViewModel;
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>() where TViewModel: BaseViewModel;
        Task InsertAsync<TViewModel>(TViewModel viewModel) where TViewModel: BaseViewModel;  
        Task UpdateAsync<TViewModel>(TViewModel viewModel) where TViewModel: BaseViewModel;
        Task DeleteAsync(TKey id);
    }
}