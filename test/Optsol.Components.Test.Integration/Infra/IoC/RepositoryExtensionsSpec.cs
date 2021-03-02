﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Utils.Data.Contexts;
using Optsol.Components.Test.Utils.Repositories.Core;
using System;
using Xunit;

namespace Optsol.Components.Test.Unit.Infra.IoC
{
    public class RepositoryExtensionsSpec
    {
        [Trait("Configuração IoC", "Context")]
        [Fact(DisplayName = "Deve configurar o Contexto e UoW na injeção de dependencia")]
        public void Deve_Configurar_Contexto_Injecao_Dependencia()
        {
            //Given
            var services = new ServiceCollection();

            //When
            services.AddLogging();
            services.AddContext<Context>(new ContextOptionsBuilder());

            //Then                
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<Context>().Should().NotBeNull();
            provider.GetRequiredService<IUnitOfWork>().Should().NotBeNull();
        }

        [Trait("Configuração IoC", "Context")]
        [Fact(DisplayName = "Não deve configurar o Contexto e UoW na injeção de dependencia sem o serviço Loggin")]
        public void Nao_Deve_Configurar_Contexto_Injecao_Dependencia()
        {
            //Given
            var services = new ServiceCollection();

            //When
            services.AddContext<Context>(new ContextOptionsBuilder());

            //Then                
            var provider = services.BuildServiceProvider();

            Action action = () => provider.GetRequiredService<IUnitOfWork>();
            action.Should().Throw<InvalidOperationException>();
        }

        [Trait("Configuração IoC", "Repository")]
        [Fact(DisplayName = "Deve adicionar automaticamente todos os repositórios da aplicação")]
        public void Deve_Adicionar_Automatica_Todos_Repositorios_Injecao_Dependencia()
        {
            //Given
            var services = new ServiceCollection();

            //When
            services.AddLogging();
            services.AddContext<Context>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>();

            //Then                
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<ITestReadRepository>().Should().NotBeNull();
            provider.GetRequiredService<ITestWriteRepository>().Should().NotBeNull();
        }

        [Trait("Configuração IoC", "Repository")]
        [Fact(DisplayName = "Não Deve adicionar todos os repositórios sem configuração do contexto")]
        public void Nao_Deve_Adicionar_Automatica_Todos_Repositorios_Injecao_Dependencia()
        {
            //Given
            var services = new ServiceCollection();

            //When
            services.AddRepository<ITestReadRepository, TestReadRepository>();

            //Then                
            var provider = services.BuildServiceProvider();

            Action action = () => provider.GetRequiredService<ITestReadRepository>();
            action.Should().Throw<InvalidOperationException>();
        }
    }
}
