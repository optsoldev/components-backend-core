using System.Collections.Generic;
using Optsol.Components.Application.ViewModel;

namespace Optsol.Components.Service.Response
{
    public class Response
    {
        public bool Success { get; set; }
        public bool Failure { get; set; }

        public IEnumerable<string> Messages { get; set; }

        public Response()
        {
            
        }

        public Response(bool success)
        {
            Success = success;
            Failure = !Success;
        }

        public Response(bool success, IEnumerable<string> messages)
            : this(success)
        {
            Messages = messages;
        }
    }
    
    public class Response<TViewModel> : Response
        where TViewModel : BaseViewModel
    {
        public TViewModel Data { get; set; }

        public Response()
        {
            
        }

        public Response(TViewModel data, bool success)
            : base(success)
        {
            Data = data;
            Success = success;
            Failure = !Success;
        }

        public Response(TViewModel data, bool success, IEnumerable<string> messages)
            : this(data, success)
        {
            Messages = messages;
        }
    }

    public class ResponseList<TViewModel> : Response
        where TViewModel: BaseViewModel
    {
        public IEnumerable<TViewModel> DataList { get; set; }

        public ResponseList()
        {
            
        }
        
        public ResponseList(IEnumerable<TViewModel> dataList, bool success) 
            : base(success)
        {
            DataList = dataList;
        }

        public ResponseList(IEnumerable<TViewModel> dataList, bool success, IEnumerable<string> messages)
            : this(dataList, success)
        {
            Messages = messages;
        }
    }
}
