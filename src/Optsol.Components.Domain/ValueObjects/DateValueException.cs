using System;

namespace Optsol.Components.Domain.ValueObjects;

[Serializable]
public class DateValueException : Exception
{
    public DateValueException(string message) : base(message)
    {
    }
}