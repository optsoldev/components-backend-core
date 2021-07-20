using FluentValidation;
using Optsol.Components.Test.Utils.ViewModels;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestSearchOnlyDtoContract : AbstractValidator<TestSearchOnlyDto>
    {
        public TestSearchOnlyDtoContract()
        {
            //TODO: REVER
            //Requires()
            //    .IsNotNull(testSearchOnlyDto.Nome, nameof(testSearchOnlyDto.Nome), "O nome do cliente não pode ser nulo")
            //    .IsNullOrEmpty(testSearchOnlyDto.SobreNome, nameof(testSearchOnlyDto.SobreNome), "O sobrenome do cliente não pode ser nulo");
        }
    }
}
