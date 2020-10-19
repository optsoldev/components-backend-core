using Flunt.Notifications;
using Optsol.Components.Application.ViewModel;
using System.Collections.Generic;

namespace Optsol.Components.Application.Result
{
    public class ServiceResult : Notifiable { }

    public class ServiceResut<TViewModel> : ServiceResult
        where TViewModel: BaseViewModel
    {
        public TViewModel Data { get; private set; }

        public ServiceResut(TViewModel data)
        {
            Data = data;

            Data.Validate();
            AddNotifications(data);
        }
    }

    public class ServiceResultList<TViewModel> : ServiceResult
        where TViewModel: BaseViewModel
    {
        public IEnumerable<TViewModel> DataList { get; private set; }

        public ServiceResultList(IEnumerable<TViewModel> dataList)
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
