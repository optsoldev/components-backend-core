using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Test.Utils.Contracts;
using System;

namespace Optsol.Components.Test.Utils.ViewModels
{
    public class UpdateTestViewModel : BaseDataTransferObject
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Contato { get; set; }

        public override void Validate()
        {
            AddNotifications(new UpdateTestViewModelContract(this));
        }
    }
}
