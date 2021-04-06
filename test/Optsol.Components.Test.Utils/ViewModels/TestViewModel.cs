using Flunt.Validations;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Test.Utils.Contracts;
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
            AddNotifications(new TestViewModelContract(this));
        }
    }
}
