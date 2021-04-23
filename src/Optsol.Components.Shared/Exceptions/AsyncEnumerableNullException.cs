using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class AsyncEnumerableNullException: Exception
    {  
        public AsyncEnumerableNullException()
            : base ("O argumento IAsyncEnumerable está nulo")
        {
            
        }
    }
}
