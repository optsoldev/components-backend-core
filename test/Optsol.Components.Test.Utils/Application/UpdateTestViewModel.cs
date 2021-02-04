using System;
using Flunt.Validations;
using Optsol.Components.Application.DataTransferObjects;

namespace Optsol.Components.Test.Utils.Application
{
    public class UpdateTestViewModel: BaseDataTransferObject
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Contato { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(Id, "Id", "O Id não pode ser nulo")
                .HasMinLen(Nome, 3, "NomeValueObject.Nome", "O nome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome, 70, "NomeValueObject.Nome", "O nome deve ter no máximo 35 caracteres")
                .IsEmail(Contato, "NomeValueObject.SobreNome", "O sobrenome deve ter no máximo 35 caracteres")
                );
        }
    }
}
