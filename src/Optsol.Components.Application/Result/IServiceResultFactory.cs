using Optsol.Components.Application.DataTransferObject;
using System.Collections.Generic;

namespace Optsol.Components.Application.Result
{
    public interface IServiceResultFactory
    {
        ServiceResult Create();
        ServiceResult<TDto> Create<TDto>(TDto dto) where TDto : BaseDataTransferObject;
        ServiceResultList<TDto> Create<TDto>(IEnumerable<TDto> dto) where TDto : BaseDataTransferObject;
    }
}
