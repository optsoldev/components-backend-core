using Flunt.Validations;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Domain.Validators
{
    public class CartaoCreditoEntityContract : Contract<CartaoCreditoEntity>
    {
        public CartaoCreditoEntityContract(CartaoCreditoEntity cartaoCreditoEntity)
        {
            Requires()
                .IsNotNullOrEmpty(cartaoCreditoEntity.NomeCliente, "NomeCliente", "O Nome do cliente não pode ser nulo")
                .IsNotNullOrEmpty(cartaoCreditoEntity.Numero, "Numero", "O Numero não pode ser nulo")
                .IsNotNullOrEmpty(cartaoCreditoEntity.CodigoVerificacao, "CodigoVerificacao", "O Codigo Verificacao do cliente não pode ser nulo")
                .IsNotEmpty(cartaoCreditoEntity.ClienteId, "ClienteId", "O Nome do cliente não pode ser nulo");
        }
    }
}
