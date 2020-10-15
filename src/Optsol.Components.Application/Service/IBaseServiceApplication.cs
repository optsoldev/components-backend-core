using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Components.Domain;

namespace Optsol.Components.Application.Service
{
    public interface IBaseServiceApplication<TEntity, TKey>
        where TEntity: IAggregateRoot<TKey>
    {        
        Task<TViewModel> GetByIdAsync<TViewModel>(TKey id);
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();
        Task InsertAsync<TViewModel>(TViewModel viewModel);  
        Task UpdateAsync<TViewModel>(TViewModel viewModel);
        Task DeleteAsync(TKey id);
    }
}