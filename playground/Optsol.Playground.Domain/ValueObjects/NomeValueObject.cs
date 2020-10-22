using Flunt.Validations;
using Optsol.Components.Domain;

namespace Optsol.Playground.Domain.ValueObjects
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
            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Nome, 3, "NomeValueObject.Nome", "O nome deve ter no mínino 3 caracteres")
                .HasMinLen(SobreNome, 3, "NomeValueObject.SobreNome", "O sobrenome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome, 35, "NomeValueObject.Nome", "O nome deve ter no máximo 35 caracteres")
                .HasMaxLen(SobreNome, 35, "NomeValueObject.SobreNome", "O sobrenome deve ter no máximo 35 caracteres")
                );
        }
    }
}
