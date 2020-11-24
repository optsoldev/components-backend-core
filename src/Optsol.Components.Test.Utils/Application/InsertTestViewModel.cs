using Flunt.Validations;
using Optsol.Components.Application.DataTransferObject;

namespace Optsol.Components.Test.Utils.Application
{
    public class InsertTestViewModel: BaseDataTransferObject
    {
        public string Nome { get; set; }
        public string Contato { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Nome, 3, "NomeValueObject.Nome", "O nome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome, 70, "NomeValueObject.Nome", "O nome deve ter no máximo 35 caracteres")
                .IsEmail(Contato, "NomeValueObject.SobreNome", "O sobrenome deve ter no máximo 35 caracteres")
                );
        }
    }
}
