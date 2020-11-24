using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Optsol.Components.Test.Shared.Utils
{
    public class XunitLogger<T> : ILogger<T>, IDisposable
    {
        public List<string> Logs = new List<string>();

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Logs.Add(state.ToString());
        }
    }
}
