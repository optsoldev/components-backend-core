using Flunt.Validations;
using Optsol.Components.Domain.ValueObjects;
using System;

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
                .HasMinLen(Nome, 3, $"{nameof(NomeValueObject.Nome)}", "O nome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome, 35, $"{nameof(NomeValueObject.Nome)}", "O nome deve ter no máximo 35 caracteres")
                .HasMinLen(SobreNome, 3, $"{nameof(NomeValueObject.SobreNome)}", "O sobrenome deve ter no mínino 3 caracteres")
                .HasMaxLen(SobreNome, 35, $"{nameof(NomeValueObject.SobreNome)}", "O sobrenome deve ter no máximo 35 caracteres")
                );
        }

        public bool Constains(string nome)
        {
            return nome.Contains(nome) || SobreNome.Contains(nome);
        }
    }
}
