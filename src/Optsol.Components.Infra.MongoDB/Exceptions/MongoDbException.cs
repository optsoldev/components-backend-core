using System;

namespace Optsol.Components.Infra.MongoDB.Exceptions;

public class MongoDbException : Exception
{
    public MongoDbException(string message) : base(message)
    {
    }
}