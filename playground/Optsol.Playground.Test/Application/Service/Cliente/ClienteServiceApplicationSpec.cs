using System.Linq;
using System;
using AutoMapper;
using FluentAssertions;
using Optsol.Components.Infra.UoW;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;
using Optsol.Playground.Domain.Repositories.Cliente;
using Optsol.Playground.Domain.ValueObjects;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Playground.Infra.Data.Context;
using Optsol.Playground.Infra.Data.Repositories.Cliente;
using Optsol.Playground.Application.Mappers.Cliente;
using Optsol.Playground.Application.ViewModels;
using System.Threading.Tasks;
using Optsol.Components.Shared.Extensions;

namespace Optsol.Playground.Test
{
    public class ClienteServiceApplicationSpec
    {
        protected IServiceProvider _serviceProvider { get; private set; }

        public ClienteServiceApplicationSpec()
        {
            ConfigurationDependencyInjection();
        }

        private void ConfigurationDependencyInjection()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddContext<PlaygroundContext>(new ContextOptionsBuilder());
            services.AddRepository<IClienteReadRepository, ClienteReadRepository>("Optsol.Playground.Domain", "Optsol.Playground.Infra");
            services.AddApplicationServices<IClienteServiceApplication, ClienteServiceApplication>("Optsol.Playground.Application");
            services.AddAServices();
            services.AddAutoMapper(typeof(ClienteViewModelToEntityMapper));

            _serviceProvider = services.BuildServiceProvider();
        }
        
        [Fact]
        public async Task Deve_Inserir_Cliente_Sem_Cartao()
        {
            //Given
            var clienteServiceApplication = _serviceProvider.GetRequiredService<IClienteServiceApplication>();

            var insertClienteViewModel = new InsertClienteViewModel();
            insertClienteViewModel.Nome = new NomeObjectViewModel { Nome = "Weslley", SobreNome = "Bruno" };
            insertClienteViewModel.Email = new EmailObjetcViewModel { Email = "weslley@outlook.com" };

            //When
            await clienteServiceApplication.InsertAsync(insertClienteViewModel);

            //Then
            var clienteReadRepository = _serviceProvider.GetRequiredService<IClienteReadRepository>();
            var clientes = await clienteReadRepository.GetAllAsync().AsyncEnumerableToEnumerable();

            clientes.Should().HaveCount(1);
            var cliente = clientes.FirstOrDefault();
            cliente.Nome.Nome.Should().Be(insertClienteViewModel.Nome.Nome);
            cliente.Nome.SobreNome.Should().Be(insertClienteViewModel.Nome.SobreNome);
            cliente.Email.Email.Should().Be(insertClienteViewModel.Email.Email);
            cliente.PossuiCartao.Should().BeFalse();
            cliente.Ativo.Should().BeFalse();
            cliente.Valid.Should().BeTrue();
            
        }

        [Fact]
        public async Task Deve_Inserir_Cartao_No_Cliente()
        {
            //Given
            var clienteWriteRepository = _serviceProvider.GetRequiredService<IClienteWriteRepository>();
            var clienteServiceApplication = _serviceProvider.GetRequiredService<IClienteServiceApplication>();
            var uow = _serviceProvider.GetRequiredService<IUnitOfWork>();

            var clienteEntity = new ClienteEntity(new NomeValueObject("Weslley", "Bruno"), new EmailValueObject("weslley@outlook.com.br"));

            var insertCartaoCreditoViewModel = new InsertCartaoCreditoViewModel();
            insertCartaoCreditoViewModel.ClienteId = clienteEntity.Id;
            insertCartaoCreditoViewModel.NomeCliente = "Weslley B. Carneiro";
            insertCartaoCreditoViewModel.Numero = "12345687415241548";
            insertCartaoCreditoViewModel.CodigoVerificacao = "854";
            insertCartaoCreditoViewModel.Validade = new DateTime(2030, 11, 30);

            //When
            await clienteWriteRepository.InsertAsync(clienteEntity);
            await uow.CommitAsync();
            
            await clienteServiceApplication.InserirCartaoNoClienteAsync(insertCartaoCreditoViewModel);

            //Then
            var clienteReadRepository = _serviceProvider.GetRequiredService<IClienteReadRepository>();
            var clientes = await clienteReadRepository.GetClientesComCartaoCreditoAsync().AsyncEnumerableToEnumerable();

            clientes.Should().HaveCount(1);
            
            var cartoes = clientes.FirstOrDefault().Cartoes;
            cartoes.Should().HaveCount(1);
            
            var cartao = cartoes.FirstOrDefault();
            cartao.ClienteId.Should().Be(insertCartaoCreditoViewModel.ClienteId);
            cartao.NomeCliente.Should().Be(insertCartaoCreditoViewModel.NomeCliente);
            cartao.Numero.Should().Be(insertCartaoCreditoViewModel.Numero);
            cartao.CodigoVerificacao.Should().Be(insertCartaoCreditoViewModel.CodigoVerificacao);
            cartao.Validade.Should().Be(insertCartaoCreditoViewModel.Validade);
            cartao.Valido.Should().BeTrue();
            cartao.Valid.Should().BeTrue();
        }
    }
}
