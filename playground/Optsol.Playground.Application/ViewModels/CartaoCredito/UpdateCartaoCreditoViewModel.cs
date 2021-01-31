using System;
using Flunt.Validations;
using Optsol.Components.Application.DataTransferObjects;

namespace Optsol.Playground.Application.ViewModels.CartaoCredito
{
    public class UpdateCartaoCreditoViewModel : BaseDataTransferObject
    {
        public Guid Id { get; set; }
        public string NomeCliente { get; set; }
        public string Numero { get; set; }
        public string CodigoVerificacao { get; set; }
        public string Validade { get; set; }
        public Guid ClienteId { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract());
        }
    }
}
