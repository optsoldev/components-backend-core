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
        public async Task DeveTestarAConfiguracaoDoServico()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository>("Optsol.Components.Test.Utils");
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            await writeRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();
            var modelResult = await serviceApplication.GetAllAsync<TestViewModel>();

            //Then            
            modelResult.DataList.Should().HaveCount(1);
            modelResult.DataList.First().Nome.Should().Be(entity.Nome.ToString());
            modelResult.DataList.First().Contato.Should().Be(entity.Email.ToString());
        }

        [Fact]
        public async Task DeveBuscarTodosRegistroPeloServico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            await serviceApplication.InsertAsync(model);
            await serviceApplication.InsertAsync(model);
            await serviceApplication.InsertAsync(model);

            var modelResult = await serviceApplication.GetAllAsync<TestViewModel>();

            //Then
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            modelResult.DataList.Should().HaveCount(3);
            modelResult.DataList.Where(w => w.Nome.Equals(model.Nome)).Should().HaveCount(3);
            modelResult.DataList.Where(w => w.Contato.Equals(model.Contato)).Should().HaveCount(3);
        } 

         [Fact]
        public async Task DeveBuscarRegistroPorIdPeloServico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            await serviceApplication.InsertAsync(model);
            var list = await serviceApplication.GetAllAsync<TestViewModel>();
            var modelResult = await serviceApplication.GetByIdAsync<TestViewModel>(list.DataList.Single().Id);

            //Then
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            modelResult.Should().NotBeNull();
            modelResult.Data.Nome.Should().Be(model.Nome);
            modelResult.Data.Contato.Should().Be(model.Contato);
        } 

        [Fact]
        public async Task DeveInserirRegistroPeloServico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            var modelResult = await serviceApplication.InsertAsync(model);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            entity.DataList.Should().HaveCount(1);
            entity.DataList.Single().Id.Should().NotBeEmpty();
            entity.DataList.Single().Nome.Should().Be(model.Nome);
            entity.DataList.Single().Contato.Should().Be(model.Contato);
            entity.DataList.Single().Ativo.Should().Be("Inativo");

        }

        [Fact]
        public async Task DeveAtualizarRegistroPeloServico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();
            
            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync<TestViewModel>()).DataList.Single();
            var updateModel = new UpdateTestViewModel();
            updateModel.Id = data.Id;
            updateModel.Nome = $"Weslley Alterado";
            updateModel.Contato = model.Contato;

            //When
            var modelResult = await serviceApplication.UpdateAsync<UpdateTestViewModel>(updateModel);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            entity.DataList.Should().HaveCount(1);
            entity.DataList.Single().Id.Should().NotBeEmpty();
            entity.DataList.Single().Nome.Should().Be(updateModel.Nome);
            entity.DataList.Single().Contato.Should().Be(updateModel.Contato);
            entity.DataList.Single().Ativo.Should().Be("Inativo");

        }

        [Fact]
        public async Task DeveRemoverRegistroPeloIdPeloServico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();
            
            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync<TestViewModel>()).DataList.Single();
            
            //When
            var modelResult = await serviceApplication.DeleteAsync(data.Id);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            modelResult.Invalid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(0);

            entity.DataList.Should().HaveCount(0);
        }

        [Fact]
        public async Task NaoDeveInserirRegistroPeloServico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "";
            model.Contato = "weslley.carneiro";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            var modelResult = await serviceApplication.InsertAsync(model);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            modelResult.Invalid.Should().BeTrue();
            modelResult.Notifications.Should().HaveCount(3);
        }

        [Fact]
        public async Task NaoDeveAtualizarRegistroPeloServico()
        {
            //Given
            InsertTestViewModel model = new InsertTestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();
            
            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync<TestViewModel>()).DataList.Single();
            var updateModel = new UpdateTestViewModel();
            updateModel.Id = data.Id;
            updateModel.Nome = "";
            updateModel.Contato = "weslley.carneiro";

            //When
            var modelResult = await serviceApplication.UpdateAsync(updateModel);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            modelResult.Invalid.Should().BeTrue();
            modelResult.Valid.Should().BeFalse();
            modelResult.Notifications.Should().HaveCount(3);
        }
    }
}
