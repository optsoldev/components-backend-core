using System;

namespace Optsol.Components.Infra.Storage.Queue.Options
{
    public interface IMessageOptions
    {
        TimeSpan? TimeToLive { get; }

        TimeSpan? InitialVisibilyDelay { get; }

        //QueueRequestOptions RequestOptions { get; }

        //OperationContext OperationContext { get; }
    }
}
