using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Optsol.Components.Test.Shared.Logger
{
    public class XunitLogger<T> : ILogger<T>, IDisposable
    {
        private bool _disposed = false;

        public readonly List<string> Logs = new();

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            _disposed = disposing;
            Console.WriteLine(_disposed);
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
