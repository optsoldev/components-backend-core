using Flunt.Validations;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class NomeValueObjectContract : Contract<NomeValueObject>
    {
        public NomeValueObjectContract(NomeValueObject nomeValueObject)
        {
            Requires()
                .IsBetween(nomeValueObject.Nome.Length, 3, 70, nameof(nomeValueObject.Nome), "O nome deve conter de 3 a 70 caracteres")
                .IsBetween(nomeValueObject.SobreNome.Length, 3, 70, nameof(nomeValueObject.SobreNome), "O nome deve conter de 3 a 70 caracteres");
        }
    }
}
