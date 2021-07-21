using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Shared.Extensions;
using Optsol.Playground.Application.Mappers.Cliente;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;
using Optsol.Playground.Domain.Repositories.Cliente;
using Optsol.Playground.Domain.ValueObjects;
using Optsol.Playground.Infra.Data.Context;
using Optsol.Playground.Infra.Data.Repositories.Cliente;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Playground.Test
{
    public class ClienteServiceApplicationSpec
    {
        protected IServiceProvider _serviceProvider;

        public ClienteServiceApplicationSpec()
        {
            ConfigurationDependencyInjection();
        }

        private void ConfigurationDependencyInjection()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddContext<PlaygroundContext>(options =>
            {
                options
                    .EnabledInMemory()
                    .EnabledLogging();

                options
                    .ConfigureRepositories<IClientePessoaFisicaReadRepository, ClienteReadRepository>("Optsol.Playground.Domain", "Optsol.Playground.Infra");
            });
            services.AddDomainNotifications();
            services.AddApplications(options =>
            {
                options
                    .ConfigureServices<IClienteServiceApplication, ClienteServiceApplication>("Optsol.Playground.Application");
            });
            services.AddServices();
            services.AddAutoMapper(typeof(ClienteViewModelToEntityMapper));

            _serviceProvider = services.BuildServiceProvider();
        }

        [Trait("Playground", "Application")]
        [Fact(DisplayName = "Deve inserir cliente sem o cartao")]
        public async Task Deve_Inserir_Cliente_Sem_Cartao()
        {
            //Given
            var clienteServiceApplication = _serviceProvider.GetRequiredService<IClienteServiceApplication>();

            ClienteRequest insertClienteViewModel = new()
            {
                Nome = "Weslley",
                SobreNome = "Bruno",
                Email = "weslley@outlook.com"
            };

            //When
            await clienteServiceApplication.InsertAsync<ClienteRequest, ClienteRequest>(insertClienteViewModel);

            //Then
            var clienteReadRepository = _serviceProvider.GetRequiredService<IClientePessoaFisicaReadRepository>();
            var clientes = await clienteReadRepository.GetAllAsync();

            clientes.Should().HaveCount(1);
            var cliente = clientes.FirstOrDefault();
            cliente.Nome.Nome.Should().Be(insertClienteViewModel.Nome);
            cliente.Nome.SobreNome.Should().Be(insertClienteViewModel.SobreNome);
            cliente.Email.Email.Should().Be(insertClienteViewModel.Email);
            cliente.PossuiCartao.Should().BeFalse();
            cliente.Ativo.Should().BeFalse();
            cliente.Valid.Should().BeTrue();
            cliente.Invalid.Should().BeFalse();
        }

        [Trait("Playground", "Application")]
        [Fact(DisplayName = "Deve inserir cartão no cliente")]
        public async Task Deve_Inserir_Cartao_No_Cliente()
        {
            //Given
            var clienteWriteRepository = _serviceProvider.GetRequiredService<IClientePessoaFisicaWriteRepository>();
            var clienteServiceApplication = _serviceProvider.GetRequiredService<IClienteServiceApplication>();
            var uow = _serviceProvider.GetRequiredService<IUnitOfWork>();

            var ClientePessoaFisicaEntity = new ClientePessoaFisicaEntity(new NomeValueObject("Weslley", "Bruno"), new EmailValueObject("weslley@outlook.com.br"), "000.000.000-00");

            CartaoCreditoRequest insertCartaoCreditoViewModel = new()
            {
                ClienteId = ClientePessoaFisicaEntity.Id,
                NomeCliente = "Weslley B. Carneiro",
                Numero = "12345687415241548",
                CodigoVerificacao = "854",
                Validade = new DateTime(2030, 11, 30)
            };

            //When
            await clienteWriteRepository.InsertAsync(ClientePessoaFisicaEntity);
            await uow.CommitAsync();

            await clienteServiceApplication.InserirCartaoNoClienteAsync(insertCartaoCreditoViewModel);

            //Then
            var clienteReadRepository = _serviceProvider.GetRequiredService<IClientePessoaFisicaReadRepository>();
            var clientes = await clienteReadRepository.BuscarClientesComCartaoCreditoAsync().AsyncEnumerableToEnumerable();

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
            cartao.Invalid.Should().BeFalse();
        }
    }
}
