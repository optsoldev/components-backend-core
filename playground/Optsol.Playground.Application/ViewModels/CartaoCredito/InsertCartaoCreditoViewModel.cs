using System;
using Flunt.Validations;
using Optsol.Components.Application.DataTransferObject;

namespace Optsol.Playground.Application.ViewModels.CartaoCredito
{
    public class InsertCartaoCreditoViewModel : BaseDataTransferObject
    {
        public string NomeCliente { get; set; }
        public string Numero { get; set; }
        public string CodigoVerificacao { get; set; }
        public DateTime Validade { get; set; }
        public Guid ClienteId { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract());
        }
    }
}
