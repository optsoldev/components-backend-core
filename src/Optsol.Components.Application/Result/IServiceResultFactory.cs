using Optsol.Components.Application.ViewModel;
using System.Collections.Generic;

namespace Optsol.Components.Application.Result
{
    public interface IServiceResultFactory
    {
        ServiceResult Create();
        ServiceResult<TViewModel> Create<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel;
        ServiceResultList<TViewModel> Create<TViewModel>(IEnumerable<TViewModel> viewModels) where TViewModel : BaseViewModel;
    }
}
