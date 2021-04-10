using Microsoft.Extensions.Logging;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Twilio.Clients;
using Optsol.Components.Infra.Twilio.Common;
using Optsol.Components.Infra.Twilio.Models;
using Optsol.Components.Shared.Extensions;
using System;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Optsol.Components.Infra.Twilio.Whatsapp
{
    public class TwilioWhatsapp : BaseTwilio, ITwilioWhatsapp
    {
        protected readonly ILogger<TwilioWhatsapp> _logger;
        protected readonly TwilioRestClient _twilioClient;

        public TwilioWhatsapp(ILogger<TwilioWhatsapp> logger, TwilioRestClient twilioRestClient)
            : base(twilioRestClient)
        {
            _logger = logger;
            _logger?.LogInformation("Iniciando o serviço TwilioSms");

            _twilioClient = twilioRestClient ?? throw new ArgumentNullException(nameof(twilioRestClient));
        }

        public Task<MessageResource> Send(SendMessageModel<string> sendMessageModel)
        {
            _logger?.LogInformation($"Send({typeof(SendMessageModel<string>)}) Return({typeof(MessageResource)})");

            return SendWhats(sendMessageModel.From, sendMessageModel.To, sendMessageModel.Data);
        }

        public Task<MessageResource> Send(SendMessageModel<IEntity> sendMessageModel)
        {
            _logger?.LogInformation($"Send({typeof(SendMessageModel<IEntity>)}) Return({typeof(MessageResource)})");

            return SendWhats(sendMessageModel.From, sendMessageModel.To, sendMessageModel.Data.ToJson());
        }

        public Task<MessageResource> Send(SendMessageModel<BaseDataTransferObject> sendMessageModel)
        {
            _logger?.LogInformation($"Send({typeof(SendMessageModel<BaseDataTransferObject>)}) Return({typeof(MessageResource)})");

            return SendWhats(sendMessageModel.From, sendMessageModel.To, sendMessageModel.Data.ToJson());
        }
    }
}
