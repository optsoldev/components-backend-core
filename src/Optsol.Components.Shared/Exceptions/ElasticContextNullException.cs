﻿using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class ElasticContextNullException : Exception
    {
        public ElasticContextNullException()
            : base("O parametro ElasticContext não foi resolvido pela injeção de dependência")
        {

        }

        protected ElasticContextNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
