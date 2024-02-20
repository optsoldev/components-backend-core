using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Test.Utils.Data.Contexts;
using Optsol.Components.Test.Utils.Entity.Entities;
using Optsol.Components.Test.Utils.Seed;
using Optsol.Components.Test.Utils.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Integration.Application
{
    public class BaseServiceApplicationSpec
    {
        private static ServiceProvider GetProviderConfiguredServicesFromContext()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddAutoMapper(typeof(TestResponseDto));
            services.AddDomainNotifications();
            services.AddContext<Context>(options =>
            {
                options
                    .EnabledInMemory()
                    .EnabledLogging();
            });
            services.AddApplications(options =>
            {
                options
                    .ConfigureServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");
            });

            return services.BuildServiceProvider();
        }

        [Trait("Servi�o de Aplica��o", "Execu��o dos Servi�os")]
        [Fact(DisplayName = "Deve obter todos os registros pelo servi�o da aplica��o")]
        public async Task Deve_Obter_Todos_Registro_Pelo_Servico()
        {
            //Given
            var numberItems = 4;
            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInContext(numberItems);

            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            var viewModels = await serviceApplication.GetAllAsync<TestResponseDto>();

            //Then
            viewModels.Should().NotBeEmpty();
            viewModels.Should().HaveCount(numberItems);
            viewModels.All(w => w.Valid).Should().BeTrue();
            viewModels.All(w => w.Invalid).Should().BeFalse();
        }

        [Trait("Servi�o de Aplica��o", "Execu��o dos Servi�os")]
        [Fact(DisplayName = "Deve obter todos os registros pelo servi�o da aplica��o")]
        public async Task Deve_Obter_Registro_Por_Id_Pelo_Servico()
        {
            //Given
            var numberItems = 4;
            var entity = default(TestEntity);

            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInContext(numberItems, (testEntityList) =>
                {
                    entity = testEntityList.First();
                });

            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            var viewModel = await serviceApplication.GetByIdAsync<TestResponseDto>(entity.Id);

            //Then
            viewModel.Should().NotBeNull();
            viewModel.Nome.Should().Be(entity.Nome.ToString());
            viewModel.Contato.Should().Be(entity.Email.ToString());

            viewModel.Validate();
            viewModel.Valid.Should().BeTrue();
            viewModel.Invalid.Should().BeFalse();
            viewModel.Notifications.Should().BeEmpty();
        }

        private class InserirNovosRegistrosParams : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new TestRequestDto()
                    {
                        Nome = "Weslley Carneiro",
                        Contato = "weslley.carneiro@optsol.com.br"
                    }
                };

                yield return new object[]
                {
                    new TestRequestDto()
                    {
                        Nome = "Felipe Carvalho",
                        Contato = "felipe.carvalho@optsol.com.br"
                    }
                };

                yield return new object[]
                {
                    new TestRequestDto()
                    {
                        Nome = "Romulo Louzada",
                        Contato = "romulo.louzada@optsol.com.br"
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Servi�o de Aplica��o", "Execu��o dos Servi�os")]
        [Theory(DisplayName = "Deve inserir registro na base de dados")]
        [ClassData(typeof(InserirNovosRegistrosParams))]
        public void Deve_Inserir_Registro_Pelo_Servico(TestRequestDto testRequestDto)
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext();

            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            var notificationContext = provider.GetRequiredService<NotificationContext>();

            //When
            Action action = () => serviceApplication.InsertAsync<TestRequestDto, TestResponseDto>(testRequestDto);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeFalse();
            notificationContext.Notifications.Should().HaveCount(0);
        }

        [Trait("Servi�o de Aplica��o", "Execu��o dos Servi�os")]
        [Fact(DisplayName = "Deve atualizar registro obtido na base de dados")]
        public async Task Deve_Atualizar_Registro_Pelo_Servico()
        {
            //Given
            var entity = default(TestEntity);

            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInContext(afterInsert: (testEntityList) =>
                {
                    entity = testEntityList.First();
                });

            var notificationContext = provider.GetRequiredService<NotificationContext>();
            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            var updateModel = new TestRequestDto();
            updateModel.Nome = $"{entity.Nome.Nome} Alterado";
            updateModel.Contato = entity.Email.ToString();

            //When
            Action action = () => serviceApplication.UpdateAsync<TestRequestDto, TestResponseDto>(entity.Id, updateModel);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeFalse();
            notificationContext.Notifications.Should().HaveCount(0);

            var viewModelResult = await serviceApplication.GetByIdAsync<TestResponseDto>(entity.Id);
            viewModelResult.Should().NotBeNull();

            viewModelResult.Valid.Should().BeTrue();
            viewModelResult.Invalid.Should().BeFalse();
            viewModelResult.Notifications.Should().BeEmpty();

            viewModelResult.Id.Should().Be(entity.Id);
            viewModelResult.Nome.Should().Be(updateModel.Nome);
            viewModelResult.Contato.Should().Be(updateModel.Contato);
        }

        [Trait("Servi�o de Aplica��o", "Execu��o dos Servi�os")]
        [Fact(DisplayName = "Deve remover registro obtido na base de dados")]
        public async Task Deve_Remover_Registro_Pelo_Id_Pelo_Servico()
        {
            //Given
            var entity = default(TestEntity);

            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInContext(afterInsert: (testEntityList) =>
                {
                    entity = testEntityList.First();
                });

            var notificationContext = provider.GetRequiredService<NotificationContext>();
            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            Action action = () => serviceApplication.DeleteAsync(entity.Id);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeFalse();
            notificationContext.Notifications.Should().BeEmpty();

            var viewModelResult = await serviceApplication.GetByIdAsync<TestResponseDto>(entity.Id);
            viewModelResult.Should().BeNull();
        }

        private class InserirNovosRegistrosComFalhasParams : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new TestRequestDto()
                    {
                        Nome = "",
                        Contato = "weslley.carneiro@optsol.com.br"
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(TestRequestDto.Nome)
                    },
                    1 //expectedErrosCount
                };

                yield return new object[]
                {
                    new TestRequestDto()
                    {
                        Nome = "Felipe Carvalho",
                        Contato = ""
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(TestRequestDto.Contato)
                    },
                    1 //expectedErrosCount
                };

                yield return new object[]
                {
                    new TestRequestDto()
                    {
                        Nome = "",
                        Contato = "mail.invalido"
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(TestRequestDto.Nome),
                        nameof(TestRequestDto.Contato)
                    },
                    2 //expectedErrosCount
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Servi�o de Aplica��o", "Execu��o dos Servi�os")]
#if DEBUG
        [Theory(DisplayName = "Não deve inserir registro na base de dados")]
#elif RELEASE
        [Theory(DisplayName = "Não deve inserir registro na base de dados", Skip = "mongo local docker test")]
#endif
        [ClassData(typeof(InserirNovosRegistrosComFalhasParams))]
        public void Nao_Deve_Inserir_Registro_Pelo_Servico(TestRequestDto viewModel, string[] expectedErrorProperty, int expectedErrosCount)
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext();

            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            var notificationContext = provider.GetRequiredService<NotificationContext>();

            //When
            Action action = () => serviceApplication.InsertAsync<TestRequestDto, TestRequestDto>(viewModel);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeTrue();
            notificationContext.Notifications.Should().HaveCount(expectedErrosCount);
            notificationContext.Notifications.Any(a => expectedErrorProperty.Contains(a.Key)).Should().BeTrue();
        }

        private class AtualizarRegistrosComFalhasParams : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new TestRequestDto()
                    {
                        Nome = "",
                        Contato = "weslley.carneiro@optsol.com.br"
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(TestRequestDto.Nome)
                    },
                    1 //expectedErrosCount
                };

                yield return new object[]
                {
                    new TestRequestDto()
                    {
                        Nome = "Felipe Carvalho",
                        Contato = ""
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(TestRequestDto.Contato)
                    },
                    1 //expectedErrosCount
                };

                yield return new object[]
                {
                    new TestRequestDto()
                    {
                        Nome = "",
                        Contato = "mail.invalido"
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(TestRequestDto.Nome),
                        nameof(TestRequestDto.Contato)
                    },
                    2 //expectedErrosCount
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Serviço de Aplicação", "Execução dos Serviços")]
        [Theory(DisplayName = "Não deve atualizar os registros obtidos na base de dados")]
        [ClassData(typeof(AtualizarRegistrosComFalhasParams))]
        public void Nao_Deve_Atualizar_Registro_Pelo_Servico(TestRequestDto viewModel, string[] expectedErrorProperty, int expectedErrosCount)
        {
            //Given
            Guid viewModelId = default;
            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInContext(afterInsert: (testEntityList) =>
                {
                    viewModelId = testEntityList.First().Id;
                });

            var notificationContext = provider.GetRequiredService<NotificationContext>();
            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            Action action = () => serviceApplication.UpdateAsync<TestRequestDto, TestResponseDto>(viewModelId, viewModel);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeTrue();
            notificationContext.Notifications.Should().HaveCount(expectedErrosCount);
            notificationContext.Notifications.Any(a => expectedErrorProperty.Contains(a.Key)).Should().BeTrue();
        }
    }
}
