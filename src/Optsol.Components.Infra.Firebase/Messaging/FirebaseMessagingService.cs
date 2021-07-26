using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Infra.Firebase.Clients;
using Optsol.Components.Infra.Firebase.Models;
using AutoMapper;
using System;
using System.Threading.Tasks;
using FirebaseAdmin;

namespace Optsol.Components.Infra.Firebase.Messaging
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

        public async Task SendAsync(PushMessage message)
        {
            if (!(message is MessageBase)) {
                _logger?.LogError($"Mensagem de tipo não suportado.Tipo esperado: { nameof(MessageBase)}");
                throw new Exception($"Mensagem de tipo não suportado. Tipo esperado: {nameof(MessageBase)}");
            }
            
            var pushMessage = _mapper.Map<Message>(message);
            var response = await _messaging.SendAsync(pushMessage);

            _logger?.LogInformation($"Resposta: {response.ToJson()}");
        }
    }
}
