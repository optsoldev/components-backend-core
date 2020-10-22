using Flunt.Notifications;
using Optsol.Components.Application.DataTransferObject;
using System.Collections.Generic;

namespace Optsol.Components.Application.Result
{
    public class ServiceResult : Notifiable { }

    public class ServiceResult<TDto> : ServiceResult
        where TDto: BaseDataTransferObject
    {
        public TDto Data { get; private set; }

        public ServiceResult(TDto data)
        {
            Data = data;

            Data.Validate();
            AddNotifications(data);
        }
    }

    public class ServiceResultList<TDto> : ServiceResult
        where TDto: BaseDataTransferObject
    {
        public IEnumerable<TDto> DataList { get; private set; }

        public ServiceResultList(IEnumerable<TDto> dataList)
        {
            DataList = dataList;

            foreach (var data in DataList)
            {
                data.Validate();
                AddNotifications(data);
            }
        }
    }
}
