using Flunt.Validations;
using Optsol.Components.Application.DataTransferObjects;

namespace Optsol.Playground.Application.ViewModels.Cliente
{
    public class ClienteViewModel : BaseDataTransferObject
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract());
        }
    }
}
