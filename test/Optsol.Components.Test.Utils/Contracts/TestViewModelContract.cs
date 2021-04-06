using Flunt.Validations;
using Optsol.Components.Test.Utils.ViewModels;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestViewModelContract : Contract<TestViewModel>
    {
        public TestViewModelContract(TestViewModel testViewModel)
        {
            Requires()
                .IsEmail(testViewModel.Contato, nameof(testViewModel.Contato), "O contato não é um email válido")
                .IsBetween(testViewModel.Nome.Length, 3, 70, nameof(testViewModel.Nome), "O nome deve conter de 3 a 70 caracteres");
        }
    }
}
