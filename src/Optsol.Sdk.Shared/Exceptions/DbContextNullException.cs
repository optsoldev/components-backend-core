using System;

namespace Optsol.Sdk.Shared.Exceptions
{
    public class DbContextNullException : Exception
    {  
        public DbContextNullException()
            : base ("O parametro contexto está nulo.")
        {
            
        }
    }
}
