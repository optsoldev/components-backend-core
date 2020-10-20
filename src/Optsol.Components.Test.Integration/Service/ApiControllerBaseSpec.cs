using System.Net;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Optsol.Components.Service;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Data;
using Xunit;
using FluentAssertions;
using Optsol.Components.Shared.Extensions;
using Newtonsoft.Json;
using System.Linq;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Service.Response;

namespace Optsol.Components.Test.Integration.Service
{
    public class ApiControllerBaseSpec
    {
        [Fact]
        public async Task DeveBuscarTodosPelaController()
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
            services.AddRepository<ITestReadRepository>("Optsol.Components.Test.Utils");
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, Guid> controllerBase = new ApiControllerBase<TestEntity, Guid>(
                provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, Guid>>>(), 
                provider.GetRequiredService<IResponseFactory>(),
                provider.GetRequiredService<IServiceApplication>());

            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            var repository = provider.GetRequiredService<ITestWriteRepository>();
            
            await repository.InsertAsync(entity);
            await repository.InsertAsync(entity2);
            await repository.InsertAsync(entity3);

            await unitOfWork.CommitAsync();

            //When
            var actionResult = await controllerBase.GetAllAsync<TestViewModel>();
            
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
        public async Task DeveBuscarRegistroPorIdPelaController()
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
            services.AddRepository<ITestReadRepository>("Optsol.Components.Test.Utils");
            services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, Guid> controllerBase = new ApiControllerBase<TestEntity, Guid>(
                provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, Guid>>>(), 
                provider.GetRequiredService<IResponseFactory>(),
                provider.GetRequiredService<IServiceApplication>());

            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            var repository = provider.GetRequiredService<ITestWriteRepository>();
            
            await repository.InsertAsync(entity);
            await repository.InsertAsync(entity2);
            await repository.InsertAsync(entity3);

            await unitOfWork.CommitAsync();

            //When
            var actionResult = await controllerBase.GetByIdAsync<TestViewModel>(entity.Id);

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
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, Guid> controllerBase = new ApiControllerBase<TestEntity, Guid>(
                provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, Guid>>>(), 
                provider.GetRequiredService<IResponseFactory>(),
                provider.GetRequiredService<IServiceApplication>());

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
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();
            IApiControllerBase<TestEntity, Guid> controllerBase = new ApiControllerBase<TestEntity, Guid>(
                provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, Guid>>>(), 
                provider.GetRequiredService<IResponseFactory>(),
                provider.GetRequiredService<IServiceApplication>());

            await serviceApplication.InsertAsync(model);

            var data = (await serviceApplication.GetAllAsync<TestViewModel>()).DataList.Single();
            var updateModel = new UpdateTestViewModel();
            updateModel.Id = data.Id;
            updateModel.Nome = $"Weslley Alterado";
            updateModel.Contato = model.Contato;

            //When
            var actionResult = await controllerBase.UpdateAsync<UpdateTestViewModel>(updateModel);

            //Then
            ((OkObjectResult)actionResult).StatusCode.Should().NotBeNull();
            ((OkObjectResult)actionResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var resultObj = JsonConvert.DeserializeObject<Response>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().NotBeNull();
            resultObj.Success.Should().BeTrue();
            resultObj.Failure.Should().BeFalse();
            resultObj.Messages.Should().BeEmpty();

            var resultService = await serviceApplication.GetByIdAsync<TestViewModel>(updateModel.Id);
            resultService.Data.Should().NotBeNull();
            resultService.Data.Id.Should().NotBeEmpty();
            resultService.Data.Nome.Should().Be(updateModel.Nome);
            resultService.Data.Contato.Should().Be(updateModel.Contato);
            resultService.Data.Ativo.Should().Be("Inativo");

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
            services.AddAServices();
            
            var provider = services.BuildServiceProvider();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();
            IApiControllerBase<TestEntity, Guid> controllerBase = new ApiControllerBase<TestEntity, Guid>(
                provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, Guid>>>(), 
                provider.GetRequiredService<IResponseFactory>(),
                provider.GetRequiredService<IServiceApplication>());

            await serviceApplication.InsertAsync(model);
            
            var entity = (await serviceApplication.GetAllAsync<TestViewModel>()).DataList.FirstOrDefault();

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

            (await serviceApplication.GetAllAsync<TestViewModel>()).DataList.Should().BeEmpty();

        }
    }
}
