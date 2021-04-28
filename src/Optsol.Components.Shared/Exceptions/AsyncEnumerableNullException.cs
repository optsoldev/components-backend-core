using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class AsyncEnumerableNullException: Exception
    {  
        public AsyncEnumerableNullException()
            : base ("O argumento IAsyncEnumerable está nulo")
        {
            
        }

        public AsyncEnumerableNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
