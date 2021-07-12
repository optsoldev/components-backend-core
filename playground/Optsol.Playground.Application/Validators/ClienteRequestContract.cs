using Flunt.Validations;
using Optsol.Playground.Application.ViewModels.Cliente;

namespace Optsol.Playground.Application.Validators
{
    public class ClienteRequestContract : Contract<ClienteRequest>
    {
        public ClienteRequestContract(ClienteRequest clienteRequest)
        {
            Requires()
                .IsBetween(clienteRequest.Nome.Length, 1, 70, nameof(clienteRequest.Nome), "O nome deve conter de 3 a 70 caracteres")
                .IsBetween(clienteRequest.SobreNome.Length, 1, 70, nameof(clienteRequest.SobreNome), "O nome deve conter de 3 a 70 caracteres")
                .IsEmail(clienteRequest.Email, $"{nameof(clienteRequest.Email)}", "O campo email deve ser do tipo email");
        }
    }
}
