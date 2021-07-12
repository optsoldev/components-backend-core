using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Test.Utils.Contracts;

namespace Optsol.Components.Test.Utils.ViewModels
{
    public class TestRequestDto: BaseDataTransferObject
    {
        public string Nome { get; set; }
        
        public string Contato { get; set; }

        public override void Validate()
        {
            var validation = new TestRequestDtoContract();
            var resultOfValidation = validation.Validate(this);

            AddNotifications(resultOfValidation);
        }
    }
}
