using FluentValidation;
using Optsol.Playground.Application.ViewModels.Cliente;

namespace Optsol.Playground.Application.Validators
{
    public class ClienteRequestContract : AbstractValidator<ClienteRequest>
    {
        public ClienteRequestContract()
        {
            //TODO: REVER
            //Requires()
            //    .IsBetween(clienteRequest.Nome.Length, 1, 70, nameof(clienteRequest.Nome), "O nome deve conter de 3 a 70 caracteres")
            //    .IsBetween(clienteRequest.SobreNome.Length, 1, 70, nameof(clienteRequest.SobreNome), "O nome deve conter de 3 a 70 caracteres")
            //    .IsEmail(clienteRequest.Email, $"{nameof(clienteRequest.Email)}", "O campo email deve ser do tipo email");
        }
    }
}
