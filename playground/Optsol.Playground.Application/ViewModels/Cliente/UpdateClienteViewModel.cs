using Optsol.Components.Application.DataTransferObjects;
using Optsol.Playground.Application.Validators;
using System;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class UpdateClienteViewModel : BaseDataTransferObject
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string SobreNome { get; set; }

        public string Email { get; set; }

        public int Ativo { get; set; }

        public override void Validate()
        {
            AddNotifications(new UpdateClienteViewModelContract(this));
        }

    }
}
