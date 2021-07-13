using Microsoft.Extensions.Logging;
using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Infra.Firebase.Clients;
using Optsol.Components.Infra.Firebase.Models;
using Optsol.Components.Infra.Firebase.Models.Request;
using System;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Firebase.Services
{
    public class FirebaseCloudMessagingServices : IPushService
    {
        private readonly ILogger _logger;
        private readonly FirebaseClient _firebaseClient;

        public FirebaseCloudMessagingServices(FirebaseClient firebaseClient, ILogger<FirebaseCloudMessagingServices> logger)
        {
            _logger = logger;
            _logger?.LogInformation($"Iniciando {nameof(FirebaseCloudMessagingServices)}");

            _firebaseClient = firebaseClient;
        }

        public async Task SendAsync(PushMessage pushMessage)
        {
            _logger?.LogInformation($"Executando: { nameof(SendAsync) }({pushMessage.ToJson()})");

            var response = await _firebaseClient.Send(new CloudMessagingRequest<PushMessage>()
            {
                To = "",
                Notification = new CloudMessagingNotificationRequest
                {
                    Title = "",
                    Body = ""
                }
            }); ;

            _logger?.LogInformation($"Resposta: {response.ToJson()}");
        }
    }
}
