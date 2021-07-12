using Optsol.Components.Application.DataTransferObjects;
using System;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class ClienteResponse : BaseDataTransferObject
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
