using Optsol.Components.Application;

namespace Optsol.Playground.Application.ViewModels.CartaoCredito
{
    public class CartaoCreditoResponse : BaseModel
    {
        public string Id { get; set; }
        public string NomeCliente { get; set; }
        public string Numero { get; set; }
        public string CodigoVerificacao { get; set; }
        public string Validade { get; set; }
        public string ClienteId { get; set; }

        public override void Validate()
        {

        }
    }
}
