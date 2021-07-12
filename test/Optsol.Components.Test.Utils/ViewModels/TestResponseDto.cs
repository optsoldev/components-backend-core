using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Test.Utils.Contracts;
using System;

namespace Optsol.Components.Test.Utils.ViewModels
{
    public class TestResponseDto : BaseDataTransferObject
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Contato { get; set; }

        public string Ativo { get; set; }

        public override void Validate()
        {
            AddNotifications(new TestResponseDtoContract(this));
        }
    }

    public class EnviarWhatsDto
    {
        public EnviarWhatsDto()
        {

        }

        public string Mensagem { get; set; }
    }
}
