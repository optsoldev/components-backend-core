using FluentValidation;
using Optsol.Components.Test.Utils.ViewModels;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestResponseDtoContract : AbstractValidator<TestResponseDto>
    {
        public TestResponseDtoContract(TestResponseDto testViewModel)
        {
            //TODO: REVER
            //Requires()
            //    .IsEmail(testViewModel.Contato, nameof(testViewModel.Contato), "O contato não é um email válido")
            //    .IsBetween(testViewModel.Nome.Length, 3, 70, nameof(testViewModel.Nome), "O nome deve conter de 3 a 70 caracteres");
        }
    }
}
