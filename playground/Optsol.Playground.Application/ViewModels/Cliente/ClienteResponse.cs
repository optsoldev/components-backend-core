using System;
using Optsol.Components.Application;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class ClienteResponse : BaseModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; } 
        public bool Ativo { get; set; }

        public override void Validate()
        {
            
        }
    }
}
