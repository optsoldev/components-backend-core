using Flunt.Validations;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Domain.Validators
{
    public class ClienteEntityContract : Contract<ClienteEntity>
    {
        public ClienteEntityContract(ClienteEntity clienteEntity)
        {
            Requires()
                .IsNotNull(clienteEntity.Nome, "Nome", "O Nome não pode ser nulo")
                .IsNotNull(clienteEntity.Email, "Email", "O Email não pode ser nulo");
        }
    }
}
