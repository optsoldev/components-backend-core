using Optsol.Components.Application.DataTransferObjects;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using System.Collections.Generic;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class ClienteComCartoesViewModel : BaseModel
    {
        
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public ICollection<CartaoCreditoResponse> Cartoes { get; set; }

        public override void Validate()
        {
            
        }
    }
}
