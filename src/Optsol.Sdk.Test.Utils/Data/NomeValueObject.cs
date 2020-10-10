using Flunt.Validations;
using Optsol.Sdk.Domain;

namespace Optsol.Sdk.Test.Shared.Data
{
    public class NomeValueObject : ValueObject
    {
        public string Nome { get; private set; }
        public string SobreNome { get; private set; }

        public NomeValueObject(string nome, string sobreNome)
        {
            Nome = nome;
            SobreNome = sobreNome;   

            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Nome, 3, "NomeValueObject.Nome", "O nome deve ter no mínino 3 caracteres")
                .HasMinLen(SobreNome, 3, "NomeValueObject.SobreNome", "O sobrenome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome, 35, "NomeValueObject.Nome", "O nome deve ter no máximo 35 caracteres")
                .HasMaxLen(SobreNome, 35, "NomeValueObject.SobreNome", "O sobrenome deve ter no máximo 35 caracteres")
                );
        }

        public override string ToString()
        {
            return $"{ Nome } { SobreNome }";
        }
    }
}
