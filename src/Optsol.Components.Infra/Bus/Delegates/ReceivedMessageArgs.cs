using Optsol.Components.Infra.Bus.Events;
using System;

namespace Optsol.Components.Infra.Bus.Delegates
{
    public class ReceivedMessageEventArgs : EventArgs
    {
        public ReceivedMessageEventArgs(IEvent message)
        {
            Message = message;
        }

        public IEvent Message { get; private set; }
    }
}
