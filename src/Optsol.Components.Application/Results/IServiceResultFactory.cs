using Optsol.Components.Application.DataTransferObjects;
using System.Collections.Generic;

namespace Optsol.Components.Application.Results
{
    public interface IServiceResultFactory
    {
        ServiceResult Create();
        ServiceResult<TDto> Create<TDto>(TDto dto) where TDto : BaseDataTransferObject;
        ServiceResultList<TDto> Create<TDto>(IEnumerable<TDto> dto) where TDto : BaseDataTransferObject;
    }
}
