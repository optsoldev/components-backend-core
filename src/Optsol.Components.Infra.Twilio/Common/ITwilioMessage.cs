using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Twilio.Models;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Optsol.Components.Infra.Twilio.Common
{
    public interface ITwilioMessage
    {
        Task<MessageResource> Send(SendMessageModel<string> sendMessageModel);

        Task<MessageResource> Send(SendMessageModel<IEntity> sendMessageModel);

        Task<MessageResource> Send(SendMessageModel<BaseDataTransferObject> sendMessageModel);
    }
}
