using Flunt.Validations;
using Optsol.Components.Application.DataTransferObject;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class InsertClienteViewModel : BaseDataTransferObject
    {
        public string Nome { get; set; }

        public string SobreNome { get; set; }

        public string Email { get; set; }

        public override void Validate()
        {
           AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Nome, 3, $"{nameof(InsertClienteViewModel.Nome)}", "O nome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome, 70, $"{nameof(InsertClienteViewModel.Nome)}", "O nome deve ter no máximo 35 caracteres")
                .HasMinLen(SobreNome, 3, $"{nameof(InsertClienteViewModel.SobreNome)}", "O sobrenome deve ter no mínino 3 caracteres")
                .HasMaxLen(SobreNome, 70, $"{nameof(InsertClienteViewModel.SobreNome)}", "O sobrenome deve ter no máximo 35 caracteres")
                .IsEmail(Email, $"{nameof(InsertClienteViewModel.Email)}", "O campo email deve ser do tipo email")
            );
        }
    }
}
