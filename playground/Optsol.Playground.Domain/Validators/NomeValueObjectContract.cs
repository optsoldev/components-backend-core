using Flunt.Validations;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Domain.Validators
{
    public class NomeValueObjectContract : Contract<NomeValueObject>
    {
        public NomeValueObjectContract(NomeValueObject nomeValueObject)
        {
            Requires()
                .IsBetween(nomeValueObject.Nome.Length, 3, 70, $"{nameof(NomeValueObject.Nome)}", "O nome deve conter de 3 a 70 caracteres")
                .IsBetween(nomeValueObject.SobreNome.Length, 3, 70, $"{nameof(NomeValueObject.SobreNome)}", "O nome deve conter de 3 a 70 caracteres");
        }
    }
}
