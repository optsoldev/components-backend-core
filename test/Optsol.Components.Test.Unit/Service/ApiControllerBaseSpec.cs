using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.Components.Infra.Data;
using Optsol.Components.Service.Controllers;
using Optsol.Components.Service.Responses;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Test.Shared.Logger;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Entity.Entities;
using Optsol.Components.Test.Utils.Service;
using Optsol.Components.Test.Utils.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Unit.Service
{
    public class ApiControllerBaseSpec
    {
        [Fact]
        public async Task Deve_Registrar_Logs_No_ApiController()
        {
            //Given
            var model = new TestViewModel();
            var insertViewModel = new InsertTestViewModel();
            var updateViewModel = new UpdateTestViewModel();

            var testSearchDto = new TestSearchDto();
            testSearchDto.Nome = "Nome";
            testSearchDto.SobreNome = "Sobrenome";
            var searchDto = new SearchRequest<TestSearchDto>();
            searchDto.Search = testSearchDto;

            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro"),
                new EmailValueObject("weslley.carneiro@outlook.com"));

            model.Id = entity.Id;

            var logger = new XunitLogger<
                ApiControllerBase<TestEntity
                , TestViewModel
                , TestViewModel
                , InsertTestViewModel
                , UpdateTestViewModel
                , TestSearchDto>>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<TestViewModel>(It.IsAny<TestEntity>())).Returns(model);
            mapperMock.Setup(mapper => mapper.Map<TestEntity>(It.IsAny<TestViewModel>())).Returns(entity);

            var mockApplicationService = new Mock<ITestServiceApplication>();

            var mockResponseFactory = new Mock<IResponseFactory>();
            mockResponseFactory.Setup(setup => setup.Create()).Returns(new Response());
            mockResponseFactory.Setup(setup => setup.Create(It.IsAny<TestViewModel>())).Returns(new Response<TestViewModel>(model, true));
            mockResponseFactory.Setup(setup => setup.Create(It.IsAny<IEnumerable<TestViewModel>>())).Returns(new ResponseList<TestViewModel>(new[] { model }, true));
            mockResponseFactory.Setup(setup => setup.Create(It.IsAny<SearchResult<TestViewModel>>())).Returns(new ResponseSearch<TestViewModel>(new SearchResult<TestViewModel>(1, 10) { Items = new[] { model } }, true));

            var controller = new TestController(loggerFactoryMock.Object, mockApplicationService.Object, mockResponseFactory.Object);

            //When
            await controller.GetAllAsync();
            await controller.GetAllAsync(searchDto);
            await controller.GetByIdAsync(model.Id);
            await controller.InsertAsync(insertViewModel);
            await controller.UpdateAsync(updateViewModel);
            await controller.DeleteAsync(model.Id);

            //Then
            var msgContructor = "Inicializando Controller Base<TestEntity, Guid>";
            var msgGetById = $"Método: GetByIdAsync({{ id:{ entity.Id } }})";
            var msgGetAllAsync = "Método: GetAllAsync() Retorno: IActionResult";
            var msgGetAllPaginatedAsync = $"Método: GetAllAsync({ searchDto.ToJson() }) Retorno: IActionResult";
            var msgInsertAsync = $"Método: InsertAsync({{ viewModel:{ insertViewModel.ToJson() } }})";
            var msgUpdateAsync = $"Método: UpdateAsync({{ viewModel:{ updateViewModel.ToJson() } }})";
            var msgDeleteAsync = $"Método: DeleteAsync({{ id:{ entity.Id } }})";

            logger.Logs.Should().HaveCount(7);
            logger.Logs.Any(a => a.Equals(msgGetById)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgContructor)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetAllAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetAllPaginatedAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgInsertAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgUpdateAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgDeleteAsync)).Should().BeTrue();
        }
    }
}
