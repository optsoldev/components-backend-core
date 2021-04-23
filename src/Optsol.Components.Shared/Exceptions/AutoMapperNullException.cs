using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public sealed class AutoMapperNullException: Exception
    {  
        public AutoMapperNullException()
            : base ("O parametro mapper não foi resolvido pela injeção de dependência")
        {
            
        }
    }
}
