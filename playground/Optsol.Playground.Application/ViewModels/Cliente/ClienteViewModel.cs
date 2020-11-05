using System;
using System.Collections.Generic;
using Flunt.Validations;
using Optsol.Components.Application.DataTransferObject;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class ClienteViewModel : BaseDataTransferObject
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public ICollection<CartaoCreditoEntity> Cartoes { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract());
        }
    }
}
