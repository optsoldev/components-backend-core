using Flunt.Validations;
using Optsol.Playground.Application.ViewModels.Cliente;

namespace Optsol.Playground.Application.Validators
{
    public class InsertClienteViewModelContract : Contract<InsertClienteViewModel>
    {
        public InsertClienteViewModelContract(InsertClienteViewModel insertClienteViewModel)
        {
            Requires()
                .IsBetween(insertClienteViewModel.Nome.Length, 1, 70, nameof(insertClienteViewModel.Nome), "O nome deve conter de 3 a 70 caracteres")
                .IsBetween(insertClienteViewModel.SobreNome.Length, 1, 70, nameof(insertClienteViewModel.SobreNome), "O nome deve conter de 3 a 70 caracteres")
                .IsEmail(insertClienteViewModel.Email, $"{nameof(insertClienteViewModel.Email)}", "O campo email deve ser do tipo email");
        }
    }
}
