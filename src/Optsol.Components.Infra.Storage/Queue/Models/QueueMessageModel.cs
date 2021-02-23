using Azure.Storage.Queues.Models;

namespace Optsol.Components.Infra.Storage.Queue.Messages
{
    public class SendMessageModel<TData>
    {
        public string QueueName { get; set; }

        public TData Data { get; set; }
    }

    public class UpdateMessageModel<TData> : SendMessageModel<TData>
    {
        public QueueMessage Message { get; set; }
    }

    public class ReceiveMessageModel<TData> : SendMessageModel<TData>
    {
        public QueueMessage Message { get; set; }
    }

    public class DeleteMessageModel
    {
        public string QueueName { get; set; }

        public QueueMessage Message { get; set; }
    }
}
