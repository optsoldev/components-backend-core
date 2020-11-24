using System;
using System.Linq;
using FluentAssertions;
using Optsol.Playground.Domain.Entidades;
using Optsol.Playground.Domain.ValueObjects;
using Xunit;

namespace Optsol.Playground.Test.Domain.Entities
{
    public class CartaoCreditoEntitySpec
    {
         [Fact]
        public void Deve_Inserir_Cartao_No_Cliente()
        {
            //Given
            ClienteEntity clienteEntity = new ClienteEntity(
                new NomeValueObject("Weslley", "Carneiro"),
                new EmailValueObject("weslley@outlook.com")
            );
            CartaoCreditoEntity cartaoCreditoEntity = new CartaoCreditoEntity(
                "Weslley B. Carneiro",
                "1326554545455",
                "985",
                new DateTime(2030, 11, 30),
                clienteEntity.Id
            );

            //When
            clienteEntity.AdicionarCartao(cartaoCreditoEntity);

            //Then
            clienteEntity.Cartoes.Any().Should().BeTrue();
        }

        [Fact]
        public void Deve_Inserir_Cartao_Vencido()
        {
            //Given
            var clienteId = Guid.NewGuid();
            ClienteEntity clienteEntity = new ClienteEntity(
                clienteId,
                new NomeValueObject("Weslley", "Carneiro"),
                new EmailValueObject("weslley@outlook.com")
            );
            CartaoCreditoEntity cartaoCreditoEntity = new CartaoCreditoEntity(
                Guid.NewGuid(),
                "Weslley B. Carneiro",
                "1326554545455",
                "985",
                new DateTime(2019, 11, 30), clienteId
            );

            //When
            clienteEntity.AdicionarCartao(cartaoCreditoEntity);

            //Then
            clienteEntity.Cartoes.FirstOrDefault().Valido.Should().BeFalse();
        }
    }
}
