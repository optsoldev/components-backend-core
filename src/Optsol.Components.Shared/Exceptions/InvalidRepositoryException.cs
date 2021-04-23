using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public sealed class InvalidRepositoryException : Exception
    {
        public InvalidRepositoryException()
            : base("O repositório foi configurado incorretamente")
        {

        }
    }
}
