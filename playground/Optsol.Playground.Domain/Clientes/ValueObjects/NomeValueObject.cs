using Optsol.Components.Domain.ValueObjects;

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
            //AddNotifications(new NomeValueObjectContract(this)); TODO: REVER
        }

        public bool Constains(string nome)
        {
            return nome.Contains(nome) || SobreNome.Contains(nome);
        }
    }
}
