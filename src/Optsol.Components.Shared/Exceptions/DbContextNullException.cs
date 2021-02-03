using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class DbContextNullException : Exception
    {  
        public DbContextNullException()
            : base ("O parametro DBContext não foi resolvido pela injeção de dependência")
        {
            
        }
    }
}
