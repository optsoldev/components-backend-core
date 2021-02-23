using System;

namespace Optsol.Components.Infra.Storage.Queue.Options
{
    public class MessageOptions : IMessageOptions
    {
        public MessageOptions(TimeSpan? timeToLive, TimeSpan? initialVisibilyDelay)
        {
            TimeToLive = timeToLive;
            InitialVisibilyDelay = initialVisibilyDelay;
        }

        //public MessageOptions(TimeSpan? timeToLive, TimeSpan? initialVisibilyDelay, QueueRequestOptions requestOptions)
        //{
        //    TimeToLive = timeToLive;
        //    InitialVisibilyDelay = initialVisibilyDelay;
        //    RequestOptions = requestOptions;
        //}

        //public MessageOptions(TimeSpan? timeToLive, TimeSpan? initialVisibilyDelay, QueueRequestOptions requestOptions, OperationContext operationContext)
        //{
        //    TimeToLive = timeToLive;
        //    InitialVisibilyDelay = initialVisibilyDelay;
        //    RequestOptions = requestOptions;
        //    OperationContext = operationContext;
        //}

        public TimeSpan? TimeToLive { get; private set; }

        public TimeSpan? InitialVisibilyDelay { get; private set; }

        //public QueueRequestOptions RequestOptions { get; private set; }

        //public OperationContext OperationContext { get; private set; }
    }
}
