using Azure.Storage.Queues.Models;

namespace Optsol.Components.Infra.Storage.Queue.Messages
{
    public class SendMessageModel<TData>
    {
        public TData Data { get; set; }

        public QueueMessage Message { get; set; }

        public SendMessageModel(TData data)
        {
            Data = data;
        }
    }

    public class UpdateMessageModel<TData> : SendMessageModel<TData>
    {
        public UpdateMessageModel(TData data) : base(data)
        {
        }
    }

    public class ReceiveMessageModel<TData> : SendMessageModel<TData>
    {
        public ReceiveMessageModel(TData data) : base(data)
        {
        }
    }

    public class DeleteMessageModel
    {
        public QueueMessage Message { get; set; }
    }
}
