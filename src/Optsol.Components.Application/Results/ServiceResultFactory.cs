using Optsol.Components.Application.DataTransferObjects;
using System.Collections.Generic;

namespace Optsol.Components.Application.Results
{
    public class ServiceResultFactory : IServiceResultFactory
    {
        public ServiceResult<TDto> Create<TDto>(TDto viewModel)
            where TDto : BaseDataTransferObject
        {
            return new ServiceResult<TDto>(viewModel);
        }

        public ServiceResultList<TDto> Create<TDto>(IEnumerable<TDto> viewModels)
            where TDto : BaseDataTransferObject
        {
            return new ServiceResultList<TDto>(viewModels);
        }

        public ServiceResult Create()
        {
            return new ServiceResult();
        }
    }
}
