using Flunt.Validations;
using Optsol.Components.Test.Utils.ViewModels;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestSearchDtoContract : Contract<TestSearchDto>
    {
        public TestSearchDtoContract(TestSearchDto testSearchDto)
        {
            Requires()
                .IsNotNull(testSearchDto.Nome, nameof(testSearchDto.Nome), "O nome do cliente não pode ser nulo")
                .IsNullOrEmpty(testSearchDto.SobreNome, nameof(testSearchDto.SobreNome), "O sobrenome do cliente não pode ser nulo");
        }
    }
}
