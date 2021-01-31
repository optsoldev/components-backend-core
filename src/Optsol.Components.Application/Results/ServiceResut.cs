using Flunt.Notifications;
using Optsol.Components.Application.DataTransferObjects;
using System.Collections.Generic;

namespace Optsol.Components.Application.Results
{
    public class ServiceResult { }

    public class ServiceResult<TDto> : ServiceResult
        where TDto : BaseDataTransferObject
    {
        public TDto Data { get; private set; }

        public ServiceResult(TDto data)
        {
            Data = data;
        }
    }

    public class ServiceResultList<TDto> : ServiceResult
        where TDto : BaseDataTransferObject
    {
        public IEnumerable<TDto> Data { get; private set; }

        public ServiceResultList(IEnumerable<TDto> data)
        {
            Data = data;
        }
    }
}
