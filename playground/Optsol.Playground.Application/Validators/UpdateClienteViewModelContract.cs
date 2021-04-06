using Flunt.Validations;
using Optsol.Playground.Application.ViewModels.Cliente;

namespace Optsol.Playground.Application.Validators
{
    public class UpdateClienteViewModelContract : Contract<UpdateClienteViewModel>
    {
        public UpdateClienteViewModelContract(UpdateClienteViewModel updateClienteViewModel)
        {
            Requires()
                .IsBetween(updateClienteViewModel.Nome.Length, 1, 70, nameof(updateClienteViewModel.Nome), "O nome deve conter de 3 a 70 caracteres")
                .IsBetween(updateClienteViewModel.SobreNome.Length, 1, 70, nameof(updateClienteViewModel.SobreNome), "O nome deve conter de 3 a 70 caracteres")
                .IsEmail(updateClienteViewModel.Email, $"{nameof(updateClienteViewModel.Email)}", "O campo email deve ser do tipo email");
        }
    }
}
