using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class InvalidRepositoryException : Exception
    {
        public InvalidRepositoryException()
            : base("O repositório foi configurado incorretamente")
        {

        }
    }
}
