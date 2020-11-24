using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Data;
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
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            await writeRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();
            var modelResult = await serviceApplication.GetAllAsync();

            //Then            
            modelResult.DataList.Should().HaveCount(1);
            modelResult.DataList.First().Nome.Should().Be(entity.Nome.ToString());
            modelResult.DataList.First().Contato.Should().Be(entity.Email.ToString());
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
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            await serviceApplication.InsertAsync(model);
            await serviceApplication.InsertAsync(model);
            await serviceApplication.InsertAsync(model);

            var modelResult = await serviceApplication.GetAllAsync();

            //Then
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            modelResult.DataList.Should().HaveCount(3);
            modelResult.DataList.Where(w => w.Nome.Equals(model.Nome)).Should().HaveCount(3);
            modelResult.DataList.Where(w => w.Contato.Equals(model.Contato)).Should().HaveCount(3);
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
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            await serviceApplication.InsertAsync(model);
            var list = await serviceApplication.GetAllAsync();
            var modelResult = await serviceApplication.GetByIdAsync(list.DataList.Single().Id);

            //Then
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            modelResult.Should().NotBeNull();
            modelResult.Data.Nome.Should().Be(model.Nome);
            modelResult.Data.Contato.Should().Be(model.Contato);
        } 

        [Fact]
        public async Task Deve_Inserir_Registro_Pelo_Servico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            var modelResult = await serviceApplication.InsertAsync(model);

            //Then
            var entity = await serviceApplication.GetAllAsync();
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            entity.DataList.Should().HaveCount(1);
            entity.DataList.Single().Id.Should().NotBeEmpty();
            entity.DataList.Single().Nome.Should().Be(model.Nome);
            entity.DataList.Single().Contato.Should().Be(model.Contato);
            entity.DataList.Single().Ativo.Should().Be("Inativo");
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
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            
            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync()).DataList.Single();
            var updateModel = new UpdateTestViewModel();
            updateModel.Id = data.Id;
            updateModel.Nome = $"Weslley Alterado";
            updateModel.Contato = model.Contato;

            //When
            var modelResult = await serviceApplication.UpdateAsync(updateModel);

            //Then
            var entity = await serviceApplication.GetAllAsync();
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            entity.DataList.Should().HaveCount(1);
            entity.DataList.Single().Id.Should().NotBeEmpty();
            entity.DataList.Single().Nome.Should().Be(updateModel.Nome);
            entity.DataList.Single().Contato.Should().Be(updateModel.Contato);
            entity.DataList.Single().Ativo.Should().Be("Inativo");
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
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            
            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync()).DataList.Single();
            
            //When
            var modelResult = await serviceApplication.DeleteAsync(data.Id);

            //Then
            var entity = await serviceApplication.GetAllAsync();
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            entity.DataList.Should().HaveCount(0);
        }

        [Fact]
        public async Task Nao_Deve_Inserir_Registro_Pelo_Servico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "";
            model.Contato = "weslley.carneiro";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();

            //When
            var modelResult = await serviceApplication.InsertAsync(model);

            //Then
            var entity = await serviceApplication.GetAllAsync();
            modelResult.Invalid.Should().BeTrue();
            modelResult.Notifications.Should().HaveCount(3);
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
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            
            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync()).DataList.Single();
            var updateModel = new UpdateTestViewModel();
            updateModel.Id = data.Id;
            updateModel.Nome = "";
            updateModel.Contato = "weslley.carneiro";

            //When
            var modelResult = await serviceApplication.UpdateAsync(updateModel);

            //Then
            var entity = await serviceApplication.GetAllAsync();
            modelResult.Invalid.Should().BeTrue();
            modelResult.Valid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(3);
        }
    }
}
