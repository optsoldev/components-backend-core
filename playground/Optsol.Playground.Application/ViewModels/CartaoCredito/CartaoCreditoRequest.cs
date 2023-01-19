using System;
using Optsol.Components.Application;

namespace Optsol.Playground.Application.ViewModels.CartaoCredito
{
    public class CartaoCreditoRequest : BaseModel
    {
        public string NomeCliente { get; set; }
        public string Numero { get; set; }
        public string CodigoVerificacao { get; set; }
        public DateTime Validade { get; set; }
        public Guid ClienteId { get; set; }
        public override void Validate()
        {

        }
    }
}
