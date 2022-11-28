using Microsoft.Extensions.Logging;
using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Infra.PushNotification.Firebase.Models;
using AutoMapper;
using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using System.Collections.Generic;
using System.Linq;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.PushNotification.Firebase.Exceptions;

namespace Optsol.Components.Infra.PushNotification.Firebase.Messaging
{
    public class FirebaseMessagingService : IPushService
    {
        private readonly FirebaseMessaging _messaging;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public FirebaseMessagingService(ILogger<FirebaseMessagingService> logger, IMapper mapper)
        {
            _messaging = FirebaseMessaging.DefaultInstance;
            _logger = logger;
            _mapper = mapper;

            _logger?.LogInformation($"Iniciando {nameof(FirebaseMessagingService)}");
        }

        public async Task SendAsync(AggregateRoot message)
        {
            if(message is not IPushMessage)
            {
                _logger?.LogError($"O Aggregate root deve implementar: { nameof(IPushMessage)}");
                throw new FirebasePushNotificationException($"O Aggregate root deve implementar: { nameof(IPushMessage)}");
            }
            
            var pushMessagesInAggregate = (message as IPushMessage).GetPushMessages().Select(push => (PushMessageBase)push);

            ValidationPushMessages(pushMessagesInAggregate);

            if (!(pushMessagesInAggregate is IEnumerable<PushMessageBase>))
            {
                _logger?.LogError($"Mensagem de tipo não suportado.Tipo esperado: { nameof(PushMessageBase)}");
                throw new FirebasePushNotificationException($"Mensagem de tipo não suportado. Tipo esperado: {nameof(PushMessageBase)}");
            }

            var pushMessages = _mapper.Map<List<Message>>(pushMessagesInAggregate);

            try
            {
                var response = await _messaging.SendAllAsync(pushMessages);

                _logger?.LogInformation($"Resposta: {response.ToJson()}");
            }
            catch (Exception ex)
            {
                throw new FirebasePushNotificationException(ex.Message, ex.InnerException);
            }
        }

        private static void ValidationPushMessages(IEnumerable<PushMessageBase> pushMessagesInAggregate)
        {
            foreach (var push in pushMessagesInAggregate)
            {
                push.Validate();
            }
        }
    }
}
