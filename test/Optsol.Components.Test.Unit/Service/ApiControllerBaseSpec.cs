using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.Data.Pagination;
using Optsol.Components.Service.Controllers;
using Optsol.Components.Service.Responses;
using Optsol.Components.Test.Shared.Logger;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Entity.Entities;
using Optsol.Components.Test.Utils.Service;
using Optsol.Components.Test.Utils.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Unit.Service
{
    public class ApiControllerBaseSpec
    {
        [Trait("Api Controllers", "Log de Ocorrências")]
        [Fact(DisplayName = "Deve registrar os logs na controller da api")]
        public async Task Deve_Registrar_Logs_No_ApiController()
        {
            //Given
            var model = new TestResponseDto();
            var insertViewModel = new TestRequestDto();
            var updateViewModel = new TestRequestDto();

            var testSearchDto = new TestSearchDto
            {
                Nome = "Nome",
                SobreNome = "Sobrenome"
            };
            var searchDto = new SearchRequest<TestSearchDto>
            {
                Search = testSearchDto
            };

            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro"),
                new EmailValueObject("weslley.carneiro@outlook.com"));

            var entityId = entity.Id;

            var logger = new XunitLogger<
                ApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto>>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<TestResponseDto>(It.IsAny<TestEntity>())).Returns(model);
            mapperMock.Setup(mapper => mapper.Map<TestEntity>(It.IsAny<TestResponseDto>())).Returns(entity);

            var mockApplicationService = new Mock<ITestServiceApplication>();
            mockApplicationService.Setup(setup => setup.InsertAsync<TestRequestDto, TestResponseDto>(It.IsAny<TestRequestDto>())).ReturnsAsync(model);
            mockApplicationService.Setup(setup => setup.UpdateAsync<TestRequestDto, TestResponseDto>(It.IsAny<Guid>(), It.IsAny<TestRequestDto>())).ReturnsAsync(model);

            var mockResponseFactory = new Mock<IResponseFactory>();
            mockResponseFactory.Setup(setup => setup.Create()).Returns(new Response());
            mockResponseFactory.Setup(setup => setup.Create(It.IsAny<TestResponseDto>())).Returns(new Response<TestResponseDto>(model, true));
            mockResponseFactory.Setup(setup => setup.Create(It.IsAny<IEnumerable<TestResponseDto>>())).Returns(new ResponseList<TestResponseDto>(new[] { model }, true));
            mockResponseFactory.Setup(setup => setup.Create(It.IsAny<SearchResult<TestResponseDto>>())).Returns(new ResponseSearch<TestResponseDto>(new SearchResult<TestResponseDto>(1, 10) { Items = new[] { model } }, true));
            mockResponseFactory.Setup(setup => setup.Create(It.IsAny<TestRequestDto>())).Returns(new Response<TestRequestDto>(insertViewModel, true));
            mockResponseFactory.Setup(setup => setup.Create(It.IsAny<TestRequestDto>())).Returns(new Response<TestRequestDto>(updateViewModel, true));


            var controller = new TestController(loggerFactoryMock.Object, mockApplicationService.Object, mockResponseFactory.Object);

            //When
            await controller.GetAllAsync();
            await controller.GetAllAsync(searchDto);
            await controller.GetByIdAsync(entityId);
            await controller.InsertAsync(insertViewModel);
            await controller.UpdateAsync(entityId, updateViewModel);
            await controller.DeleteAsync(entityId);

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
