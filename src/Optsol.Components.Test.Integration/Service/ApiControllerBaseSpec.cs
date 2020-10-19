using System.Net;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Data;
using Xunit;
using FluentAssertions;
using Optsol.Components.Shared.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Application.Result;

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

            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, Guid> controllerBase = new ApiControllerBase<TestEntity, Guid>(
                provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, Guid>>>(), 
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
            var resultObj = JsonConvert.DeserializeObject<ServiceResultList<TestViewModel>>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.DataList.Should().HaveCount(3);
            resultObj.DataList.Any(a => a.Id == entity.Id).Should().BeTrue();
            resultObj.DataList.Any(a => a.Contato == entity2.Email.ToString()).Should().BeTrue();
            resultObj.DataList.Any(a => a.Nome == entity3.Nome.ToString()).Should().BeTrue();
        }
    }
}
