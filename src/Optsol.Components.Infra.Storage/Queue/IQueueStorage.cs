using Azure;
using Azure.Storage.Queues.Models;
using Optsol.Components.Infra.Storage.Queue.Messages;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Storage.Queue
{
    public interface IQueueStorage
    {
        delegate void Subscribe<T>(ReceiveMessageModel<T> messages);

        Task<Response<SendReceipt>> SendMessageAsync<TData>(SendMessageModel<TData> message) 
            where TData : class;

        Task<Response<SendReceipt>> SendMessageBase64Async<TData>(SendMessageModel<TData> message)
            where TData : class;

        Task<Response<UpdateReceipt>> UpdateMessageAsync<TData>(UpdateMessageModel<TData> message)
            where TData : class;

        Task<Response> DeleteMessageAsync<TData>(DeleteMessageModel message)
            where TData : class;

        Task<Response<QueueMessage[]>> ReceiveMessageAsync<TData>(string queueName) where TData : class;
    }
}
