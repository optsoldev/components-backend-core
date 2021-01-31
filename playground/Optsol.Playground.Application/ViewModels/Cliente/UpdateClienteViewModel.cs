using System;
using Flunt.Validations;
using Optsol.Components.Application.DataTransferObjects;

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
            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Nome, 3, $"{nameof(UpdateClienteViewModel.Nome)}", "O nome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome, 70, $"{nameof(UpdateClienteViewModel.Nome)}", "O nome deve ter no máximo 35 caracteres")
                .HasMinLen(SobreNome, 3, $"{nameof(UpdateClienteViewModel.SobreNome)}", "O sobrenome deve ter no mínino 3 caracteres")
                .HasMaxLen(SobreNome, 70, $"{nameof(UpdateClienteViewModel.SobreNome)}", "O sobrenome deve ter no máximo 35 caracteres")
                .IsEmail(Email, $"{nameof(UpdateClienteViewModel.Email)}", "O campo email deve ser do tipo email")
            );
        }

    }
}
