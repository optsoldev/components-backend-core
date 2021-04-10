using Flunt.Extensions.Br.Validations;
using Flunt.Validations;
using Optsol.Components.Infra.Twilio.Models;

namespace Optsol.Components.Infra.Twilio.Validators
{
    public class SendMessageModelContract<T> : Contract<SendMessageModel<T>>
        where T : class
    {
        public SendMessageModelContract(SendMessageModel<T> sendMessageModel)
        {
            Requires()
                .IsPhoneNumber(sendMessageModel.To, nameof(sendMessageModel.To), "O número do telefone está inválido")
                .IsPhoneNumber(sendMessageModel.From, nameof(sendMessageModel.From), "O número do telefone está inválido")
                .IsNotNull(sendMessageModel.Data, nameof(sendMessageModel.Data), "O campo data não deve ser nulo");
        }
    }
}
