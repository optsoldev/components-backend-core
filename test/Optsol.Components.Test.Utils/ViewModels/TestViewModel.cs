using Flunt.Validations;
using Optsol.Components.Application.DataTransferObjects;
using System;

namespace Optsol.Components.Test.Utils.ViewModels
{
    public class TestViewModel : BaseDataTransferObject
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Contato { get; set; }
        public string Ativo { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Nome, 3, $"{nameof(TestViewModel.Nome)}", "O nome deve ter no mínino 3 caracteres")
                .HasMaxLen(Nome, 70, $"{nameof(TestViewModel.Nome)}", "O nome deve ter no máximo 35 caracteres")
                .IsEmail(Contato, $"{nameof(TestViewModel.Contato)}", "O sobrenome deve ter no máximo 35 caracteres")
                );
        }
    }
}
