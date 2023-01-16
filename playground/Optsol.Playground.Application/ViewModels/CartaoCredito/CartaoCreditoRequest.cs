using Optsol.Components.Application.DataTransferObjects;
using System;

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
