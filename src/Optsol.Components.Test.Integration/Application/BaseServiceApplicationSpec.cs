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
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");
            
            var provider = services.BuildServiceProvider();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            await writeRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();
            var entityGet = await serviceApplication.GetAllAsync<TestViewModel>();

            //Then            
            entityGet.Should().HaveCount(1);
            entityGet.First().Nome.Should().Be(entity.Nome.ToString());
            entityGet.First().Contato.Should().Be(entity.Email.ToString());
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
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            await serviceApplication.InsertAsync(model);
            await serviceApplication.InsertAsync(model);
            await serviceApplication.InsertAsync(model);

            var modelResult = await serviceApplication.GetAllAsync<TestViewModel>();

            //Then
            ((ServiceApplication)serviceApplication).Invalid.Should().BeFalse();
            ((ServiceApplication)serviceApplication).Notifications.Should().HaveCount(0);

            modelResult.Should().HaveCount(3);
            modelResult.Where(w => w.Nome.Equals(model.Nome)).Should().HaveCount(3);
            modelResult.Where(w => w.Contato.Equals(model.Contato)).Should().HaveCount(3);
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
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            await serviceApplication.InsertAsync(model);
            var list = await serviceApplication.GetAllAsync<TestViewModel>();
            var modelResult = await serviceApplication.GetByIdAsync<TestViewModel>(list.Single().Id);

            //Then
            ((ServiceApplication)serviceApplication).Invalid.Should().BeFalse();
            ((ServiceApplication)serviceApplication).Notifications.Should().HaveCount(0);

            modelResult.Should().NotBeNull();
            modelResult.Nome.Should().Be(model.Nome);
            modelResult.Contato.Should().Be(model.Contato);
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
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            await serviceApplication.InsertAsync(model);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            ((ServiceApplication)serviceApplication).Invalid.Should().BeFalse();
            ((ServiceApplication)serviceApplication).Notifications.Should().HaveCount(0);

            entity.Should().HaveCount(1);
            entity.Single().Id.Should().NotBeEmpty();
            entity.Single().Nome.Should().Be(model.Nome);
            entity.Single().Contato.Should().Be(model.Contato);
            entity.Single().Ativo.Should().Be("Inativo");

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
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();
            
            await serviceApplication.InsertAsync(model);

            var modelResult = (await serviceApplication.GetAllAsync<TestViewModel>()).Single();
            var updateModel = new UpdateTestViewModel();
            updateModel.Id = modelResult.Id;
            updateModel.Nome = $"Weslley Alterado";
            updateModel.Contato = model.Contato;

            //When
            await serviceApplication.UpdateAsync<UpdateTestViewModel>(updateModel);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            ((ServiceApplication)serviceApplication).Invalid.Should().BeFalse();
            ((ServiceApplication)serviceApplication).Notifications.Should().HaveCount(0);

            entity.Should().HaveCount(1);
            entity.Single().Id.Should().NotBeEmpty();
            entity.Single().Nome.Should().Be(updateModel.Nome);
            entity.Single().Contato.Should().Be(updateModel.Contato);
            entity.Single().Ativo.Should().Be("Inativo");

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
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();
            
            await serviceApplication.InsertAsync(model);

            var modelResult = (await serviceApplication.GetAllAsync<TestViewModel>()).Single();
            
            //When
            await serviceApplication.DeleteAsync(modelResult.Id);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            ((ServiceApplication)serviceApplication).Invalid.Should().BeFalse();
            ((ServiceApplication)serviceApplication).Notifications.Should().HaveCount(0);

            entity.Should().HaveCount(0);
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
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();

            //When
            await serviceApplication.InsertAsync(model);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            ((ServiceApplication)serviceApplication).Invalid.Should().BeTrue();
            ((ServiceApplication)serviceApplication).Notifications.Should().HaveCount(3);
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
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();
            
            await serviceApplication.InsertAsync(model);

            var modelResult = (await serviceApplication.GetAllAsync<TestViewModel>()).Single();
            var updateModel = new UpdateTestViewModel();
            updateModel.Id = modelResult.Id;
            updateModel.Nome = "";
            updateModel.Contato = "weslley.carneiro";

            //When
            await serviceApplication.UpdateAsync<UpdateTestViewModel>(updateModel);

            //Then
            var entity = await serviceApplication.GetAllAsync<TestViewModel>();
            ((ServiceApplication)serviceApplication).Invalid.Should().BeTrue();
            ((ServiceApplication)serviceApplication).Notifications.Should().HaveCount(3);
        }
    }
}
