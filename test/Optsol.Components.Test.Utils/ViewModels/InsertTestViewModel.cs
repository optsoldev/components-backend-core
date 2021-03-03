using Flunt.Validations;
using Optsol.Components.Application.DataTransferObjects;

namespace Optsol.Components.Test.Utils.ViewModels
{
    public class InsertTestViewModel: BaseDataTransferObject
    {
        public string Nome { get; set; }
        
        public string Contato { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Nome, 3, nameof(Nome), "O nome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome, 70, nameof(Nome), "O nome deve ter no máximo 35 caracteres")
                .IsEmail(Contato, nameof(Contato), "O contato não é um email válido")
                );
        }
    }
}
