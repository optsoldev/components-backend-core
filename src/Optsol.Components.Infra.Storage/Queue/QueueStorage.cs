using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Storage.Queue.Messages;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Shared.Settings;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Storage.Queue
{
    public class QueueStorage : IQueueStorage
    {
        private QueueClient _queueClient;

        private readonly ILogger _logger;

        private readonly StorageSettings _storageSettings;

        public QueueStorage(StorageSettings settings, ILogger<QueueStorage> logger, ILogger<StorageSettingsNullException> loggerException)
        {
            _storageSettings = settings ?? throw new StorageSettingsNullException(loggerException);
            _storageSettings.Validate();

            _logger = logger;
        }

        public async Task<Response<SendReceipt>> SendMessageAsync<TData>(SendMessageModel<TData> message)
            where TData : class
        {
            await GetQueueClient(message.QueueName);

            return await _queueClient.SendMessageAsync(message.Data.ToJson());
        }

        public async Task<Response<UpdateReceipt>> UpdateMessageAsync<TData>(UpdateMessageModel<TData> message)
            where TData : class
        {
            await GetQueueClient(message.QueueName);

            return await _queueClient.UpdateMessageAsync(message.Message.MessageId, message.Message.PopReceipt, message.Data.ToJson());
        }

        public async Task<Response> DeleteMessageAsync<TData>(DeleteMessageModel message)
            where TData : class
        {
            await GetQueueClient(message.QueueName);

            return await _queueClient.DeleteMessageAsync(message.Message.MessageId, message.Message.PopReceipt);
        }

        public async Task<Response<QueueMessage[]>> ReceiveMessageAsync<TData>(string queueName)
            where TData : class
        {
            await GetQueueClient(queueName);

            return await _queueClient.ReceiveMessagesAsync();
        }

        #region private

        private async Task GetQueueClient(string queueName)
        {
            _logger.LogInformation($"Executando: {nameof(GetQueueClient)}() Retorno: Task<QueueClient>");

            var queueClientCreated = _queueClient != null;
            if (queueClientCreated)
            {
                return;
            }

            _queueClient = new QueueClient(_storageSettings.ConnectionString, queueName);

            await _queueClient.CreateIfNotExistsAsync();
        }

        #endregion
    }
}
