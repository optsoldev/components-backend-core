using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Test.Utils.Contracts;

namespace Optsol.Components.Test.Utils.ViewModels
{
    public class InsertTestViewModel: BaseDataTransferObject
    {
        public string Nome { get; set; }
        
        public string Contato { get; set; }

        public override void Validate()
        {
            AddNotifications(new InsertTestViewModelContract(this));
        }
    }
}
