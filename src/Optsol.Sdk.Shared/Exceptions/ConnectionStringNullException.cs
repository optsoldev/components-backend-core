using System;

namespace Optsol.Sdk.Shared.Exceptions
{
    public class ConnectionStringNullException: Exception
    {  
        public ConnectionStringNullException()
            : base ("A string de conexão não foi encontrada.")
        {
            
        }
    }
}
