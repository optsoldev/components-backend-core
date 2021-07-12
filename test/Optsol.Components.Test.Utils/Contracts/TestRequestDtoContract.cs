using Flunt.Validations;
using Optsol.Components.Test.Utils.ViewModels;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestRequestDtoContract : Contract<TestRequestDto>
    {
        public TestRequestDtoContract(TestRequestDto insertTestViewModel)
        {
            Requires()
                .IsEmail(insertTestViewModel.Contato, nameof(insertTestViewModel.Contato), "O contato não é um email válido")
                .IsBetween(insertTestViewModel.Nome.Length, 3, 70, nameof(insertTestViewModel.Nome), "O nome deve conter de 3 a 70 caracteres");
        }
    }
}
