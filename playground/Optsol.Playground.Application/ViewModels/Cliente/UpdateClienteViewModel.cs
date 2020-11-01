using System;
using Flunt.Validations;
using Optsol.Components.Application.DataTransferObject;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class UpdateClienteViewModel : BaseDataTransferObject
    {
        public Guid Id { get; set; }
        public NomeObjectViewModel Nome { get; set; }
        public EmailObjetcViewModel Email { get; set; }
        public int Ativo { get; set; }
        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Nome.Nome, 3, "NomeValueObject.Nome", "O nome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome.Nome, 70, "NomeValueObject.Nome", "O nome deve ter no máximo 35 caracteres")
                .HasMinLen(Nome.SobreNome, 3, "NomeValueObject.SobreNome", "O sobrenome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome.SobreNome, 70, "NomeValueObject.SobreNome", "O sobrenome deve ter no máximo 35 caracteres")
                .IsEmail(Email.Email, "EmailValueObject.Email", "O campo email deve ser do tipo email")
            );
        }

    }
}
