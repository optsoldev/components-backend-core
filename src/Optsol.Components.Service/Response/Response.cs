using System.Collections.Generic;
using System.Net;
using Flunt.Notifications;

namespace Microsoft.AspNetCore.Mvc
{
    public class Response<TData>
        where TData : class
    {
        public bool Success { get; set; }
        public bool Failure { get; set; }

        public TData Data { get; set; }
        public IEnumerable<string> Messages { get; set; }

        public Response(TData data, bool success, HttpStatusCode statusCode)
        {
            Data = data;
            Success = success;
            Failure = !Success;
        }

        public Response(TData data, bool success, HttpStatusCode statusCode, IEnumerable<string> messages)
            : this(data, success, statusCode)
        {
            Messages = messages;
        }
    }
}
