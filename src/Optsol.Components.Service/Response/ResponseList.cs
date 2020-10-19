using System.Collections.Generic;
using System.Net;

namespace Microsoft.AspNetCore.Mvc
{
    public class ResponseList<TData> : Response<IEnumerable<TData>>
    {
        public ResponseList(IEnumerable<TData> data, bool success, HttpStatusCode statusCode) 
            : base(data, success, statusCode)
        {
        }
    }
}
