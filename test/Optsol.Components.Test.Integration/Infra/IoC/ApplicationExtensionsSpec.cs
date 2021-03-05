using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Test.Utils.Data.Contexts;
using Optsol.Components.Test.Utils.Mapper;
using Optsol.Components.Test.Utils.ViewModels;
using System;
using Xunit;

namespace Optsol.Components.Test.Unit.Infra.IoC
{
    public class ApplicationExtensionsSpec
    {
        [Trait("Configuração IoC", "Cors")]
        [Fact(DisplayName = "Deve configurar o Cors na injeção de dependencia")]
        public void Deve_Configurar_Cors_Injecao_Dependencia()
        {
            //Given
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Settings/appsettings.cors.json")
                .Build();

            var services = new ServiceCollection();

            //When
            services.AddLogging();
            services.AddCors(configuration);

            //Then                
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<ICorsService>().Should().NotBeNull();
        }

        [Trait("Configuração IoC", "Cors")]
        [Fact(DisplayName = "Não deve configurar o Cors na injeção de dependencia sem a chave no appsettings.json")]
        public void Nao_Deve_Configurar_Cors_Injecao_Dependencia_Sem_Chave_AppSettings()
        {
            //Given
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Settings/appsettings.mongo.json")
                .Build();

            var services = new ServiceCollection();
            services.AddLogging();

            //When
            Action action = () => services.AddCors(configuration);

            //Then                
            action.Should().Throw<CorsSettingsNullException>();
        }

        [Trait("Configuração IoC", "Cors")]
        [Fact(DisplayName = "Não deve configurar o Cors na injeção de dependencia sem o servico Loggin")]
        public void Nao_Deve_Configurar_Cors_Injecao_Dependencia_Sem_Loggin()
        {
            //Given
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Settings/appsettings.cors.json")
                .Build();

            var services = new ServiceCollection();

            //When
            services.AddCors(configuration);

            //Then                
            var provider = services.BuildServiceProvider();

            Action action = () => provider.GetRequiredService<ICorsService>();
            action.Should().Throw<InvalidOperationException>();
        }

        [Trait("Configuração Aplicação", "Cors")]
        [Fact(DisplayName = "Deve configurar o middleware Cors na aplicação")]
        public void Deve_Configurar_Middleware_Cors_Aplicacao()
        {
            //Given
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Settings/appsettings.cors.json")
                .Build();
            
            var services = new ServiceCollection();
            services.AddCors(configuration);
            var provider = services.BuildServiceProvider();

            var appBuilder = new ApplicationBuilder(provider);

            //When
            Action action = () => appBuilder.UseCors(configuration); 
            
            //Then                
            action.Should().NotThrow();
        }

        [Trait("Configuração Aplicação", "Cors")]
        [Fact(DisplayName = "Não deve configurar o middleware Cors na aplicação sem a chave no appsettings.json")]
        public void Nao_Deve_Configurar_Middleware_Cors_Aplicacao_Sem_Chave_AppSettings()
        {
            //Given
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Settings/appsettings.mongo.json")
                .Build();

            var services = new ServiceCollection();
            services.AddLogging();
            var provider = services.BuildServiceProvider();

            var appBuilder = new ApplicationBuilder(provider);

            //When
            Action action = () => appBuilder.UseCors(configuration);

            //Then                
            action.Should().Throw<CorsSettingsNullException>();
        }

        [Trait("Configuração IoC", "Application Services")]
        [Fact(DisplayName = "Deve configurar os serviços de aplicação na injeção de dependencia")]
        public void Deve_Configurar_Application_Services_Injecao_Dependencia()
        {
            //Given
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddContext<Context>(new ContextOptionsBuilder());
            services.AddDomainNotifications();
            services.AddAutoMapper(typeof(TestEntityToViewModel));

            //When
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            //Then                
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<ITestServiceApplication>().Should().NotBeNull();
        }

        [Trait("Configuração IoC", "Application Services")]
        [Fact(DisplayName = "Não deve configurar os serviços de aplicação na injeção de dependencia sem serviço Loggin")]
        public void Nao_Deve_Configurar_Application_Services_Injecao_Dependencia_Sem_Loggin()
        {
            //Given
            var services = new ServiceCollection();
            services.AddContext<Context>(new ContextOptionsBuilder());
            services.AddDomainNotifications();
            services.AddAutoMapper(typeof(TestEntityToViewModel));

            //When
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            //Then                
            var provider = services.BuildServiceProvider();
            Action action = () => provider.GetRequiredService<ITestServiceApplication>();
            action.Should().Throw<InvalidOperationException>();
        }

        [Trait("Configuração IoC", "Application Services")]
        [Fact(DisplayName = "Não deve configurar os serviços de aplicação na injeção de dependencia sem serviço AutoMapper")]
        public void Nao_Deve_Configurar_Application_Services_Injecao_Dependencia_Sem_AutoMapper()
        {
            //Given
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddContext<Context>(new ContextOptionsBuilder());
            services.AddDomainNotifications();

            //When
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            //Then                
            var provider = services.BuildServiceProvider();
            Action action = () => provider.GetRequiredService<ITestServiceApplication>();
            action.Should().Throw<InvalidOperationException>();
        }

        [Trait("Configuração IoC", "Application Services")]
        [Fact(DisplayName = "Não deve configurar os serviços de aplicação na injeção de dependencia sem serviço DomainNotification")]
        public void Nao_Deve_Configurar_Application_Services_Injecao_Dependencia_Sem_DomainNotification()
        {
            //Given
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddContext<Context>(new ContextOptionsBuilder());
            services.AddAutoMapper(typeof(TestEntityToViewModel));

            //When
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            //Then                
            var provider = services.BuildServiceProvider();
            Action action = () => provider.GetRequiredService<ITestServiceApplication>();
            action.Should().Throw<InvalidOperationException>();
        }

        [Trait("Configuração IoC", "Application Services")]
        [Fact(DisplayName = "Não deve configurar os serviços de aplicação na injeção de dependencia sem serviço Contexto")]
        public void Nao_Deve_Configurar_Application_Services_Injecao_Dependencia_Sem_Contexto()
        {
            //Given
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestEntityToViewModel));
            services.AddDomainNotifications();

            //When
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            //Then                
            var provider = services.BuildServiceProvider();
            Action action = () => provider.GetRequiredService<ITestServiceApplication>();
            action.Should().Throw<InvalidOperationException>();
        }
    }
}
