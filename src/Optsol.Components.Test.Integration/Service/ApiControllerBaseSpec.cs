using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Optsol.Components.Service;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Data;
using Xunit;
using FluentAssertions;
using Optsol.Components.Shared.Extensions;
using Newtonsoft.Json;
using System.Linq;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Service.Response;
using Optsol.Components.Application.Service;

namespace Optsol.Components.Test.Integration.Service
{
    public class ApiControllerBaseSpec
    {
        [Fact]
        public async Task Deve_Buscar_Todos_Pela_Controller()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley_1", "Carneiro"),
                new EmailValueObject("weslley.carneiro@optsol.com.br")
            );
            var entity2 = new TestEntity(
                new NomeValueObject("Weslley_2", "Carneiro"),
                new EmailValueObject("weslley.carneiro@optsol.com.br")
            );
            var entity3 = new TestEntity(
                new NomeValueObject("Weslley_3", "Carneiro"),
                new EmailValueObject("weslley.carneiro@optsol.com.br")
            );

            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");
            services.AddApplicationServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel> controllerBase = 
                new ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>(
                    provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>>(), 
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>(),
                    provider.GetRequiredService<IResponseFactory>());

            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            var repository = provider.GetRequiredService<ITestWriteRepository>();
            
            await repository.InsertAsync(entity);
            await repository.InsertAsync(entity2);
            await repository.InsertAsync(entity3);

            await unitOfWork.CommitAsync();

            //When
            var actionResult = await controllerBase.GetAllAsync();
            
            //Then
            ((OkObjectResult)actionResult).StatusCode.Should().NotBeNull();
            ((OkObjectResult)actionResult).StatusCode.Should().Be((int)HttpStatusCode.OK);
            
            var resultObj = JsonConvert.DeserializeObject<ResponseList<TestViewModel>>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().NotBeNull();
            resultObj.Success.Should().BeTrue();
            resultObj.Failure.Should().BeFalse();
            resultObj.Messages.Should().BeEmpty();
            resultObj.DataList.Should().NotBeNull();
            resultObj.DataList.All(a => a.Valid).Should().BeTrue();
            resultObj.DataList.All(a => a.Invalid).Should().BeFalse();
            resultObj.DataList.SelectMany(s => s.Notifications).Should().BeEmpty();
            resultObj.DataList.Should().HaveCount(3);
            resultObj.DataList.Any(a => a.Id == entity.Id).Should().BeTrue();
            resultObj.DataList.Any(a => a.Contato == entity2.Email.ToString()).Should().BeTrue();
            resultObj.DataList.Any(a => a.Nome == entity3.Nome.ToString()).Should().BeTrue();
        }

        [Fact]
        public async Task Deve_Buscar_Registro_Por_Id_Pela_Controller()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley_1", "Carneiro"),
                new EmailValueObject("weslley.carneiro@optsol.com.br")
            );
            var entity2 = new TestEntity(
                new NomeValueObject("Weslley_2", "Carneiro"),
                new EmailValueObject("weslley.carneiro@optsol.com.br")
            );
            var entity3 = new TestEntity(
                new NomeValueObject("Weslley_3", "Carneiro"),
                new EmailValueObject("weslley.carneiro@optsol.com.br")
            );

            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");
            services.AddApplicationServices<IServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel> controllerBase = 
                new ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>(
                    provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>>(), 
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>(),
                    provider.GetRequiredService<IResponseFactory>());

            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            var repository = provider.GetRequiredService<ITestWriteRepository>();
            
            await repository.InsertAsync(entity);
            await repository.InsertAsync(entity2);
            await repository.InsertAsync(entity3);

            await unitOfWork.CommitAsync();

            //When
            var actionResult = await controllerBase.GetByIdAsync(entity.Id);

            //Then
            ((OkObjectResult)actionResult).StatusCode.Should().NotBeNull();
            ((OkObjectResult)actionResult).StatusCode.Should().Be((int)HttpStatusCode.OK);
            
            var resultObj = JsonConvert.DeserializeObject<Response<TestViewModel>>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().NotBeNull();
            resultObj.Success.Should().BeTrue();
            resultObj.Failure.Should().BeFalse();
            resultObj.Messages.Should().BeEmpty();
            resultObj.Data.Should().NotBeNull();
            resultObj.Data.Nome.Should().Be(entity.Nome.ToString());
            resultObj.Data.Contato.Should().Be(entity.Email.ToString());
            resultObj.Data.Ativo.Should().Be("Inativo");
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
            services.AddApplicationServices<IServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel> controllerBase = 
                new ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>(
                    provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>>(), 
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>(),
                    provider.GetRequiredService<IResponseFactory>());

            //When
            var actionResult = await controllerBase.InsertAsync(model);

            //Then
            ((OkObjectResult)actionResult).StatusCode.Should().NotBeNull();
            ((OkObjectResult)actionResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var resultObj = JsonConvert.DeserializeObject<Response>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().NotBeNull();
            resultObj.Success.Should().BeTrue();
            resultObj.Failure.Should().BeFalse();
            resultObj.Messages.Should().BeEmpty();
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
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            IApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel> controllerBase = 
                new ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>(
                    provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>>(), 
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>(),
                    provider.GetRequiredService<IResponseFactory>());

            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync()).DataList.Single();
            var updateModel = new UpdateTestViewModel();
            updateModel.Id = data.Id;
            updateModel.Nome = $"Weslley Alterado";
            updateModel.Contato = model.Contato;

            //When
            var actionResult = await controllerBase.UpdateAsync(updateModel);

            //Then
            ((OkObjectResult)actionResult).StatusCode.Should().NotBeNull();
            ((OkObjectResult)actionResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var resultObj = JsonConvert.DeserializeObject<Response>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().NotBeNull();
            resultObj.Success.Should().BeTrue();
            resultObj.Failure.Should().BeFalse();
            resultObj.Messages.Should().BeEmpty();

            var resultService = await serviceApplication.GetByIdAsync(updateModel.Id);
            resultService.Data.Should().NotBeNull();
            resultService.Data.Id.Should().NotBeEmpty();
            resultService.Data.Nome.Should().Be(updateModel.Nome);
            resultService.Data.Contato.Should().Be(updateModel.Contato);
            resultService.Data.Ativo.Should().Be("Inativo");

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
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            IApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel> controllerBase = 
                new ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>(
                    provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>>(), 
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>>(),
                    provider.GetRequiredService<IResponseFactory>());

            await serviceApplication.InsertAsync(model);
            
            var entity = (await serviceApplication.GetAllAsync()).DataList.FirstOrDefault();

            //When
            var actionResult = await controllerBase.DeleteAsync(entity.Id);

            //Then
            ((OkObjectResult)actionResult).StatusCode.Should().NotBeNull();
            ((OkObjectResult)actionResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var resultObj = JsonConvert.DeserializeObject<Response>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().NotBeNull();
            resultObj.Success.Should().BeTrue();
            resultObj.Failure.Should().BeFalse();
            resultObj.Messages.Should().BeEmpty();

            (await serviceApplication.GetAllAsync()).DataList.Should().BeEmpty();

        }
    }
}
