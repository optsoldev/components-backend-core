using Optsol.Components.Application.ViewModel;
using System.Collections.Generic;

namespace Optsol.Components.Application.Result
{
    public class ServiceResultFactory : IServiceResultFactory
    {
        public ServiceResult<TViewModel> Create<TViewModel>(TViewModel viewModel)
            where TViewModel : BaseViewModel
        {
            return new ServiceResult<TViewModel>(viewModel);
        }

        public ServiceResultList<TViewModel> Create<TViewModel>(IEnumerable<TViewModel> viewModels)
            where TViewModel : BaseViewModel
        {
            return new ServiceResultList<TViewModel>(viewModels);
        }

        public ServiceResult Create()
        {
            return new ServiceResult();
        }
    }
}
