using System.Linq;
using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;
using Optsol.Components.Infra.UoW;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;
using Optsol.Playground.Domain.Repositories.Cliente;
using Optsol.Playground.Domain.ValueObjects;
using Xunit;

namespace Optsol.Playground.Test
{
    public class ClienteServiceApplicationSpec
    {
        [Fact]
        public void Deve_Inserir_Cartao_No_Cliente()
        {
            //Given
            var clienteId = Guid.NewGuid();
            ClienteEntity clienteEntity = new ClienteEntity(clienteId, new NomeValueObject("Weslley", "Carneiro"), new EmailValueObject("weslley@outlook.com"));
            CartaoCreditoEntity cartaoCreditoEntity = new CartaoCreditoEntity(Guid.NewGuid(), "Weslley B. Carneiro", "1326554545455", "985", new DateTime(2030, 11, 30), clienteId);

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
            ClienteEntity clienteEntity = new ClienteEntity(clienteId, new NomeValueObject("Weslley", "Carneiro"), new EmailValueObject("weslley@outlook.com"));
            CartaoCreditoEntity cartaoCreditoEntity = new CartaoCreditoEntity(Guid.NewGuid(), "Weslley B. Carneiro", "1326554545455", "985", new DateTime(2019, 11, 30), clienteId);

            //When
            clienteEntity.AdicionarCartao(cartaoCreditoEntity);

            //Then
            clienteEntity.Cartoes.FirstOrDefault().Valido.Should().BeFalse();
        }
    }
}
