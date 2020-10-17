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
                new NomeValueObject("Weslley", "Carneiro"),
                new EmailValueObject("weslley.carneiro@optsol.com.br")
            );

            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IApiControllerBase<TestEntity, Guid> controllerBase = new ApiControllerBase<TestEntity, Guid>(
                provider.GetRequiredService<ILogger<ApiControllerBase<TestEntity, Guid>>>(), 
                provider.GetRequiredService<IServiceApplication>());

            var repository = provider.GetRequiredService<ITestWriteRepository>();
            await repository.InsertAsync(entity);
            
            var uow = provider.GetRequiredService<IUnitOfWork>();
            await uow.CommitAsync();

            //When
            var actionResult = await controllerBase.GetAllAsync<TestViewModel>();
            
            //Then
            ((OkObjectResult)actionResult).StatusCode.Should().NotBeNull();
            ((OkObjectResult)actionResult).StatusCode.Should().Be((int)HttpStatusCode.OK);
            var resultObj = JsonConvert.DeserializeObject<List<TestViewModel>>(((OkObjectResult)actionResult).Value.ToJson());
            resultObj.Should().HaveCount(1);
            resultObj.Any(a => a.Id == entity.Id).Should().BeTrue();
            resultObj.Any(a => a.Contato == entity.Email.ToString()).Should().BeTrue();
            resultObj.Any(a => a.Nome == entity.Nome.ToString()).Should().BeTrue();
        }
    }
}
