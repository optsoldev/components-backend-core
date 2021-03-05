using System.Collections.Generic;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Infra.Data;

namespace Optsol.Components.Service.Responses
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

    public class Response<TData> : Response
        where TData : BaseDataTransferObject
    {
        public TData Data { get; set; }

        public Response()
        {

        }

        public Response(TData data, bool success)
            : base(success)
        {
            Data = data;
            Success = success;
            Failure = !Success;
        }

        public Response(TData data, bool success, IEnumerable<string> messages)
            : this(data, success)
        {
            Messages = messages;
        }
    }

    public class ResponseList<TData> : Response
        where TData : BaseDataTransferObject
    {
        public IEnumerable<TData> Data { get; set; }

        public ResponseList()
        {

        }

        public ResponseList(IEnumerable<TData> data, bool success)
            : base(success)
        {
            Data = data;
        }

        public ResponseList(IEnumerable<TData> data, bool success, IEnumerable<string> messages)
            : this(data, success)
        {
            Messages = messages;
        }
    }

    public class ResponseSearch<TData> : ResponseList<TData>
        where TData : BaseDataTransferObject
    {
        public int Page { get; set; }

        public int? PageSize { get; set; }

        public long Total { get; set; }

        public ResponseSearch()
        {
        }

        public ResponseSearch(SearchResult<TData> data, bool success) 
            : base(data.Items, success)
        {
            SetPageData(data);
        }

        public ResponseSearch(SearchResult<TData> data, bool success, IEnumerable<string> messages) 
            : base(data.Items, success, messages)
        {
            SetPageData(data);
        }
        
        private void SetPageData(SearchResult<TData> data)
        {
            Page = (int)data.Page;
            PageSize = (int?)data.PageSize;
            Total = data.TotalItems;
        }
    }
}
