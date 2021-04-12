using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class InvalidRepositoryException : Exception
    {
        public InvalidRepositoryException()
            : base("O repositório está incorreto para a entidade utilizada")
        {

        }
    }
}
