using Optsol.Components.Application.DataTransferObjects;
using Optsol.Playground.Application.Validators;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class ClienteRequest : BaseDataTransferObject
    {
        public string Nome { get; set; }

        public string SobreNome { get; set; }

        public string Email { get; set; }

        public string Documento { get; set; }

        public override void Validate()
        {
            //TODO: REVER
            //AddNotifications(new ClienteRequestContract(this));
        }
    }
}
