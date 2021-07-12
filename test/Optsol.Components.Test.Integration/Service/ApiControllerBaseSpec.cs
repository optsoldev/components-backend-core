using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Optsol.Components.Application.Services;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Service.Controllers;
using Optsol.Components.Service.Responses;
using Optsol.Components.Test.Utils.Data.Contexts;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Entity.Entities;
using Optsol.Components.Test.Utils.Repositories.Core;
using Optsol.Components.Test.Utils.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

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

            var searchDto = new TestSearchDto();

            services.AddLogging();
            services.AddAutoMapper(typeof(TestResponseDto));
            services.AddContext<Context>(options =>
            {
                options
                    .EnabledInMemory()
                    .EnabledLogging();

                options
                    .ConfigureRepositories<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");
            });
            services.AddDomainNotifications();
            services.AddApplications(options =>
            {
                options.ConfigureServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");
            });
            services.AddServices();

            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto> controllerBase =
                new ApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto>(
                    provider.GetRequiredService<ILoggerFactory>(),
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity>>(),
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

            var resultObj = JsonConvert.DeserializeObject<ResponseList<TestResponseDto>>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().NotBeNull();
            resultObj.Success.Should().BeTrue();
            resultObj.Failure.Should().BeFalse();
            resultObj.Messages.Should().BeEmpty();
            resultObj.Data.Should().NotBeNull();
            resultObj.Data.All(a => a.IsValid).Should().BeTrue();
            resultObj.Data.SelectMany(s => s.Notifications).Should().BeEmpty();
            resultObj.Data.Should().HaveCount(3);
            resultObj.Data.Any(a => a.Id == entity.Id).Should().BeTrue();
            resultObj.Data.Any(a => a.Contato == entity2.Email.ToString()).Should().BeTrue();
            resultObj.Data.Any(a => a.Nome == entity3.Nome.ToString()).Should().BeTrue();
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

            var searchDto = new TestSearchDto();

            services.AddLogging();
            services.AddAutoMapper(typeof(TestResponseDto));
            services.AddDomainNotifications();
            services.AddContext<Context>(options =>
            {
                options
                    .EnabledInMemory()
                    .EnabledLogging();

                options
                    .ConfigureRepositories<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");
            });
            services.AddApplications(options =>
            {
                options
                    .ConfigureServices<ITestServiceApplication, TestServiceApplication>("Optsol.Components.Test.Utils");
            });
            services.AddServices();

            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto> controllerBase =
                new ApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto>(
                    provider.GetRequiredService<ILoggerFactory>(),
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity>>(),
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

            var resultObj = JsonConvert.DeserializeObject<Response<TestResponseDto>>(((OkObjectResult)actionResult).Value.ToJson());
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
            TestRequestDto model = new()
            {
                Nome = "Weslley Carneiro",
                Contato = "weslley.carneiro@optsol.com.br"
            };

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
            services.AddServices();

            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto> controllerBase =
                new ApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto>(
                    provider.GetRequiredService<ILoggerFactory>(),
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity>>(),
                    provider.GetRequiredService<IResponseFactory>());

            //When
            var actionResult = await controllerBase.InsertAsync(model);

            //Then
            ((OkObjectResult)actionResult).StatusCode.Should().NotBeNull();
            ((OkObjectResult)actionResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var resultObj = JsonConvert.DeserializeObject<Response<TestRequestDto>>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().NotBeNull();
            resultObj.Data.Should().NotBeNull();
            resultObj.Success.Should().BeTrue();
            resultObj.Failure.Should().BeFalse();
            resultObj.Messages.Should().BeEmpty();
        }

        [Fact]
        public async Task Deve_Atualizar_Registro_Pelo_Servico()
        {
            //Given
            TestRequestDto model = new()
            {
                Nome = "Weslley Carneiro",
                Contato = "weslley.carneiro@optsol.com.br"
            };

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
            services.AddServices();

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            IApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto> controllerBase =
                new ApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto>(
                    provider.GetRequiredService<ILoggerFactory>(),
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity>>(),
                    provider.GetRequiredService<IResponseFactory>());

            await serviceApplication.InsertAsync<TestRequestDto, TestRequestDto>(model);

            var data = (await serviceApplication.GetAllAsync<TestResponseDto>()).Single();
            var updateModelId = data.Id;
            var updateModel = new TestRequestDto
            {
                Nome = $"Weslley Alterado",
                Contato = model.Contato
            };

            //When
            var actionResult = await controllerBase.UpdateAsync(updateModelId, updateModel);

            //Then
            ((OkObjectResult)actionResult).StatusCode.Should().NotBeNull();
            ((OkObjectResult)actionResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var resultObj = JsonConvert.DeserializeObject<Response<TestResponseDto>>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().NotBeNull();
            resultObj.Data.Should().NotBeNull();
            resultObj.Success.Should().BeTrue();
            resultObj.Failure.Should().BeFalse();
            resultObj.Messages.Should().BeEmpty();

            var resultService = await serviceApplication.GetByIdAsync<TestResponseDto>(updateModelId);
            resultService.Should().NotBeNull();
            resultService.Id.Should().NotBeEmpty();
            resultService.Nome.Should().Be(updateModel.Nome);
            resultService.Contato.Should().Be(updateModel.Contato);
            resultService.Ativo.Should().Be("Inativo");

        }

        [Fact]
        public async Task Deve_Remover_Registro_Pelo_Id_Pelo_Servico()
        {
            //Given
            TestRequestDto model = new()
            {
                Nome = "Weslley Carneiro",
                Contato = "weslley.carneiro@optsol.com.br"
            };

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
            services.AddServices();

            var provider = services.BuildServiceProvider();
            ITestServiceApplication serviceApplication = provider.GetRequiredService<ITestServiceApplication>();
            IApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto> controllerBase =
                new ApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto>(
                    provider.GetRequiredService<ILoggerFactory>(),
                    provider.GetRequiredService<IBaseServiceApplication<TestEntity>>(),
                    provider.GetRequiredService<IResponseFactory>());

            await serviceApplication.InsertAsync<TestRequestDto, TestRequestDto>(model);

            var entity = (await serviceApplication.GetAllAsync<TestResponseDto>()).FirstOrDefault();

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

            (await serviceApplication.GetAllAsync<TestResponseDto>()).Should().BeEmpty();

        }
    }
}
