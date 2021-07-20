using Optsol.Components.Domain.ValueObjects;
using Optsol.Components.Test.Utils.Contracts;

namespace Optsol.Components.Test.Utils.Data.Entities.ValueObjecs
{
    public class NomeValueObject : ValueObject
    {
        public string Nome { get; private set; }
        public string SobreNome { get; private set; }

        public NomeValueObject(string nome, string sobreNome)
        {
            Nome = nome;
            SobreNome = sobreNome;

            Validate();
        }

        public override string ToString()
        {
            return $"{ Nome } { SobreNome }";
        }

        public override void Validate()
        {
            //TODO: REVER
            //AddNotifications(new NomeValueObjectContract(this));
        }
    }
}
