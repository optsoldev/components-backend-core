using Optsol.Components.Application.DataTransferObjects;
using Optsol.Playground.Application.Validators;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class InsertClienteViewModel : BaseDataTransferObject
    {
        public string Nome { get; set; }

        public string SobreNome { get; set; }

        public string Email { get; set; }

        public override void Validate()
        {
            AddNotifications(new InsertClienteViewModelContract(this));
        }
    }
}
