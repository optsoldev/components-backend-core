using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class UnitOfWorkNullException: Exception
    {  
        public UnitOfWorkNullException()
            : base("O parametro UnitOfWork não foi resolvido pela injeção de dependência")
        {
            
        }
    }
}
