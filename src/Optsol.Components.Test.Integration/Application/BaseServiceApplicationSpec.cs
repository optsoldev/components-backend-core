using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Integration.Application
{
    public class BaseServiceApplicationSpec
    {

        [Fact]
        public async Task Deve_Testar_A_Configuracao_Do_Servico()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddDomainNotifications();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            await writeRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();

            //When
            var viewModels = await serviceApplication.GetAllAsync();

            //Then            
            viewModels.Should().HaveCount(1);
            viewModels.First().Nome.Should().Be(entity.Nome.ToString());
            viewModels.First().Contato.Should().Be(entity.Email.ToString());
        }

        [Fact]
        public async Task Deve_Buscar_Todos_Registro_Pelo_Servico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddDomainNotifications();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            await serviceApplication.InsertAsync(model);
            await serviceApplication.InsertAsync(model);
            await serviceApplication.InsertAsync(model);

            var viewModels = await serviceApplication.GetAllAsync();

            //Then
            viewModels.Should().HaveCount(3);
            viewModels.Where(w => w.Nome.Equals(model.Nome)).Should().HaveCount(3);
            viewModels.Where(w => w.Contato.Equals(model.Contato)).Should().HaveCount(3);
        }

        [Fact]
        public async Task Deve_Buscar_Registro_Por_Id_Pelo_Servico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddDomainNotifications();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            await serviceApplication.InsertAsync(model);

            var findId = (await serviceApplication.GetAllAsync()).First().Id;

            //When
            var viewModel = await serviceApplication.GetByIdAsync(findId);

            //Then
            viewModel.Should().NotBeNull();
            viewModel.Nome.Should().Be(model.Nome);
            viewModel.Contato.Should().Be(model.Contato);
        }

        [Fact]
        public void Deve_Inserir_Registro_Pelo_Servico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddDomainNotifications();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            NotificationContext notificationContext = provider.GetRequiredService<NotificationContext>();

            //When
            Action action = () => serviceApplication.InsertAsync(model);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeFalse();
            notificationContext.Notifications.Should().HaveCount(0);
        }

        [Fact]
        public async Task Deve_Atualizar_Registro_Pelo_Servico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddDomainNotifications();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();

            NotificationContext notificationContext = provider.GetRequiredService<NotificationContext>();

            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync()).Single();
            var updateModel = new UpdateTestViewModel();
            updateModel.Id = data.Id;
            updateModel.Nome = $"Weslley Alterado";
            updateModel.Contato = model.Contato;

            //When
            Action action = () => serviceApplication.UpdateAsync(updateModel);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeFalse();
            notificationContext.Notifications.Should().HaveCount(0);
        }

        [Fact]
        public async Task Deve_Remover_Registro_Pelo_Id_Pelo_Servico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddDomainNotifications();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();

            NotificationContext notificationContext = provider.GetRequiredService<NotificationContext>();

            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync()).Single();

            //When
            Action action = () => serviceApplication.DeleteAsync(data.Id);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeFalse();
            notificationContext.Notifications.Should().HaveCount(0);
        }

        [Fact]
        public void Nao_Deve_Inserir_Registro_Pelo_Servico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "";
            model.Contato = "weslley.carneiro";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddDomainNotifications();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();

            NotificationContext notificationContext = provider.GetRequiredService<NotificationContext>();

            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            Action action = () => serviceApplication.InsertAsync(model);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeTrue();
            notificationContext.Notifications.Should().HaveCount(3);
        }

        [Fact]
        public async Task Nao_Deve_Atualizar_Registro_Pelo_Servico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddDomainNotifications();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();

            NotificationContext notificationContext = provider.GetRequiredService<NotificationContext>();

            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync()).Single();

            var updateModel = new UpdateTestViewModel();
            updateModel.Id = data.Id;
            updateModel.Nome = "";
            updateModel.Contato = "weslley.carneiro";

            //When
            Action action = () => serviceApplication.UpdateAsync(updateModel);

            //Then
            action.Should().NotThrow();

            notificationContext.HasNotifications.Should().BeTrue();
            notificationContext.Notifications.Should().HaveCount(3);
        }
    }
}
