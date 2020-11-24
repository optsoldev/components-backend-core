using Flunt.Validations;
using Optsol.Components.Application.DataTransferObject;

namespace Optsol.Playground.Application.ViewModels.CartaoCredito
{
    public class CartaoCreditoViewModel : BaseDataTransferObject
    {
        public string Id { get; set; }
        public string NomeCliente { get; set; }
        public string Numero { get; set; }
        public string CodigoVerificacao { get; set; }
        public string Validade { get; set; }
        public string ClienteId { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract());
        }
    }
}
