using FluentAssertions;
using Optsol.Playground.Domain.Entities;
using Optsol.Playground.Domain.ValueObjects;
using System;
using System.Linq;
using Xunit;

namespace Optsol.Playground.Test.Domain.Entities
{
    public class CartaoCreditoEntitySpec
    {
        [Trait("Playground", "Domain")]
        [Fact(DisplayName = "Deve inserir cartao no cliente")]
        public void Deve_Inserir_Cartao_No_Cliente()
        {
            //Given
            var clienteEntity = new ClientePessoaFisicaEntity(
                new NomeValueObject("Weslley", "Carneiro"),
                new EmailValueObject("weslley@outlook.com"),
                "000.000.000-00"
            );
            var cartaoCreditoEntity = new CartaoCreditoEntity(
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

        [Trait("Playground", "Domain")]
        [Fact(DisplayName = "Não deve inserir cartao vencido")]
        public void Nao_Deve_Inserir_Cartao_Vencido()
        {
            //Given
            var clienteId = Guid.NewGuid();
            var clienteEntity = new ClientePessoaFisicaEntity(
                clienteId,
                new NomeValueObject("Weslley", "Carneiro"),
                new EmailValueObject("weslley@outlook.com"),
                "000.000.000-00"
            );
            var cartaoCreditoEntity = new CartaoCreditoEntity(
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
