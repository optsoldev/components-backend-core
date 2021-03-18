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
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddDomainNotifications();
            services.AddContext<Context>(options =>
            {
                options
                    .EnabledInMemory()
                    .EnabledLogging();
            });
            services.AddApplications<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            return services.BuildServiceProvider();
        }

        [Trait("Serviço de Aplicação", "Execução dos Serviços")]
        [Fact(DisplayName = "Deve obter todos os registros pelo serviço da aplicação")]
        public async Task Deve_Obter_Todos_Registro_Pelo_Servico()
        {
            //Given
            var numberItems = 4;
            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInContext(numberItems);

            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            var viewModels = await serviceApplication.GetAllAsync();

            //Then
            viewModels.Should().NotBeEmpty();
            viewModels.Should().HaveCount(numberItems);
            viewModels.Any(w => w.Invalid).Should().BeFalse();
        }

        [Trait("Serviço de Aplicação", "Execução dos Serviços")]
        [Fact(DisplayName = "Deve obter todos os registros pelo serviço da aplicação")]
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
            var viewModel = await serviceApplication.GetByIdAsync(entity.Id);

            //Then
            viewModel.Should().NotBeNull();
            viewModel.Nome.Should().Be(entity.Nome.ToString());
            viewModel.Contato.Should().Be(entity.Email.ToString());

            viewModel.Validate();
            viewModel.Invalid.Should().BeFalse();
            viewModel.Valid.Should().BeTrue();
            viewModel.Notifications.Should().BeEmpty();
        }

        private class InserirNovosRegistrosParams : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new InsertTestViewModel()
                    {
                        Nome = "Weslley Carneiro",
                        Contato = "weslley.carneiro@optsol.com.br"
                    }
                };

                yield return new object[]
                {
                    new InsertTestViewModel()
                    {
                        Nome = "Felipe Carvalho",
                        Contato = "felipe.carvalho@optsol.com.br"
                    }
                };

                yield return new object[]
                {
                    new InsertTestViewModel()
                    {
                        Nome = "Romulo Louzada",
                        Contato = "romulo.louzada@optsol.com.br"
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Serviço de Aplicação", "Execução dos Serviços")]
        [Theory(DisplayName = "Deve inserir registro na base de dados")]
        [ClassData(typeof(InserirNovosRegistrosParams))]
        public void Deve_Inserir_Registro_Pelo_Servico(InsertTestViewModel viewmModel)
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext();

            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            var notificationContext = provider.GetRequiredService<NotificationContext>();

            //When
            Action action = () => serviceApplication.InsertAsync(viewmModel);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeFalse();
            notificationContext.Notifications.Should().HaveCount(0);
        }

        [Trait("Serviço de Aplicação", "Execução dos Serviços")]
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

            var updateModel = new UpdateTestViewModel();
            updateModel.Id = entity.Id;
            updateModel.Nome = $"{entity.Nome.Nome} Alterado";
            updateModel.Contato = entity.Email.ToString();

            //When
            Action action = () => serviceApplication.UpdateAsync(updateModel);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeFalse();
            notificationContext.Notifications.Should().HaveCount(0);

            var viewModelResult = await serviceApplication.GetByIdAsync(updateModel.Id);
            viewModelResult.Should().NotBeNull();

            viewModelResult.Invalid.Should().BeFalse();
            viewModelResult.Notifications.Should().BeEmpty();

            viewModelResult.Id.Should().Be(updateModel.Id);
            viewModelResult.Nome.Should().Be(updateModel.Nome);
            viewModelResult.Contato.Should().Be(updateModel.Contato);
        }

        [Trait("Serviço de Aplicação", "Execução dos Serviços")]
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

            var viewModelResult = await serviceApplication.GetByIdAsync(entity.Id);
            viewModelResult.Should().BeNull();
        }

        private class InserirNovosRegistrosComFalhasParams : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new InsertTestViewModel()
                    {
                        Nome = "",
                        Contato = "weslley.carneiro@optsol.com.br"
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(InsertTestViewModel.Nome)
                    },
                    2 //expectedErrosCount
                };

                yield return new object[]
                {
                    new InsertTestViewModel()
                    {
                        Nome = "Felipe Carvalho",
                        Contato = ""
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(InsertTestViewModel.Contato)
                    },
                    1 //expectedErrosCount
                };

                yield return new object[]
                {
                    new InsertTestViewModel()
                    {
                        Nome = "",
                        Contato = "mail.invalido"
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(InsertTestViewModel.Nome),
                        nameof(InsertTestViewModel.Contato)
                    },
                    3 //expectedErrosCount
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Serviço de Aplicação", "Execução dos Serviços")]
        [Theory(DisplayName = "Não deve inserir registro na base de dados")]
        [ClassData(typeof(InserirNovosRegistrosComFalhasParams))]
        public void Nao_Deve_Inserir_Registro_Pelo_Servico(InsertTestViewModel viewModel, string[] expectedErrorProperty, int expectedErrosCount)
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext();

            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            var notificationContext = provider.GetRequiredService<NotificationContext>();

            //When
            Action action = () => serviceApplication.InsertAsync(viewModel);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeTrue();
            notificationContext.Notifications.Should().HaveCount(expectedErrosCount);
            notificationContext.Notifications.Any(a => expectedErrorProperty.Contains(a.Property)).Should().BeTrue();
        }

        private class AtualizarRegistrosComFalhasParams : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new UpdateTestViewModel()
                    {
                        Nome = "",
                        Contato = "weslley.carneiro@optsol.com.br"
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(UpdateTestViewModel.Nome)
                    },
                    2 //expectedErrosCount
                };

                yield return new object[]
                {
                    new UpdateTestViewModel()
                    {
                        Nome = "Felipe Carvalho",
                        Contato = ""
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(UpdateTestViewModel.Contato)
                    },
                    1 //expectedErrosCount
                };

                yield return new object[]
                {
                    new UpdateTestViewModel()
                    {
                        Nome = "",
                        Contato = "mail.invalido"
                    },
                    new [] //expectedErrorProperty
                    {
                        nameof(UpdateTestViewModel.Nome),
                        nameof(UpdateTestViewModel.Contato)
                    },
                    3 //expectedErrosCount
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Serviço de Aplicação", "Execução dos Serviços")]
        [Theory(DisplayName = "Não deve atualizar os registros obtidos na base de dados")]
        [ClassData(typeof(AtualizarRegistrosComFalhasParams))]
        public void Nao_Deve_Atualizar_Registro_Pelo_Servico(UpdateTestViewModel viewModel, string[] expectedErrorProperty, int expectedErrosCount)
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInContext(afterInsert: (testEntityList) =>
                {   
                    viewModel.Id = testEntityList.First().Id;
                });

            var notificationContext = provider.GetRequiredService<NotificationContext>();
            var serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            Action action = () => serviceApplication.UpdateAsync(viewModel);

            //Then
            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeTrue();
            notificationContext.Notifications.Should().HaveCount(expectedErrosCount);
            notificationContext.Notifications.Any(a => expectedErrorProperty.Contains(a.Property)).Should().BeTrue();
        }
    }
}
