using Optsol.Components.Test.Utils.Contracts;
using System;
using Optsol.Components.Application;

namespace Optsol.Components.Test.Utils.ViewModels
{
    public class TestResponseDto : BaseModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Contato { get; set; }

        public string Ativo { get; set; }

        public override void Validate()
        {
            //TODO: REVER
            //AddNotifications(new TestResponseDtoContract(this));
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
