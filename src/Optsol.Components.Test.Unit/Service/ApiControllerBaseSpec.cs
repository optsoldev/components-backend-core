using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Optsol.Components.Service;
using Optsol.Components.Service.Response;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Test.Shared.Logger;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Data;
using Optsol.Components.Test.Utils.Service;
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

            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro"),
                new EmailValueObject("weslley.carneiro@outlook.com"));

            model.Id = entity.Id;

            var logger = new XunitLogger<ApiControllerBase<TestEntity
                , TestViewModel
                , TestViewModel
                , InsertTestViewModel
                , UpdateTestViewModel>>();

            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<TestViewModel>(It.IsAny<TestEntity>())).Returns(model);
            mapperMock.Setup(mapper => mapper.Map<TestEntity>(It.IsAny<TestViewModel>())).Returns(entity);

            var mockApplicationService = new Mock<ITestServiceApplication>();
            var mockResponseFactory = new Mock<IResponseFactory>();

            var controller = new TestController(logger, mockApplicationService.Object, mockResponseFactory.Object);

            //When
            await controller.GetAllAsync();
            await controller.GetByIdAsync(model.Id);
            await controller.InsertAsync(insertViewModel);
            await controller.UpdateAsync(updateViewModel);
            await controller.DeleteAsync(model.Id);

            //Then
            var msgContructor = "Inicializando Controller Base<TestEntity, Guid>";
            var msgGetById = $"Método: GetByIdAsync({{ id:{ entity.Id } }})";
            var msgGetAllAsync = "Método: GetAllAsync() Retorno: IActionResult";
            var msgInsertAsync = $"Método: InsertAsync({{ viewModel:{ insertViewModel.ToJson() } }})";
            var msgUpdateAsync = $"Método: UpdateAsync({{ viewModel:{ updateViewModel.ToJson() } }})";
            var msgDeleteAsync = $"Método: DeleteAsync({{ id:{ entity.Id } }})";

            logger.Logs.Should().HaveCount(6);
            logger.Logs.Any(a => a.Equals(msgGetById)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgContructor)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetAllAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgInsertAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgUpdateAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgDeleteAsync)).Should().BeTrue();
        }
    }
}
