using Flunt.Validations;
using Optsol.Components.Test.Utils.ViewModels;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestSearchOnlyDtoContract : Contract<TestSearchOnlyDto>
    {
        public TestSearchOnlyDtoContract(TestSearchOnlyDto testSearchOnlyDto)
        {
            Requires()
                .IsNotNull(testSearchOnlyDto.Nome, nameof(testSearchOnlyDto.Nome), "O nome do cliente não pode ser nulo")
                .IsNullOrEmpty(testSearchOnlyDto.SobreNome, nameof(testSearchOnlyDto.SobreNome), "O sobrenome do cliente não pode ser nulo");
        }
    }
}
