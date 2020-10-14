using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class DbContextNullException : Exception
    {  
        public DbContextNullException()
            : base ("O parametro contexto está nulo.")
        {
            
        }
    }
}
