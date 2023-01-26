using System;

namespace Optsol.Components.Domain.ValueObjects;

public class DateValue : ValueObject
{
    public DateTime Date { get; private set; }

    public DateValue()
    {
        Date = DateTime.MinValue;
    }

    public DateValue SetDateValueWithDate(DateTime date)
    {
        if (date == DateTime.MinValue)
        {
            throw new DateValueException($"{Date} not is min value");
        }

        Date = date;

        return this;
    }

    public DateValue SetDateValueWithDateOfNow()
    {
        Date = DateTime.Now;
             
        return this;
    }

    public override string ToString()
    {
        return Date.ToShortDateString();
    }
    
    public static DateValue Create() => new();
    public override void Validate()
    {
    }
}