using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Shared.Logger;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Service;
using Xunit;

namespace Optsol.Components.Test.Unit.Service
{
    public class ApiControllerBaseSpec
    {
        [Fact]
        public async Task DeveRegistrarLogsNoApiController()
        {
            //Given
            var model = new TestViewModel();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro"),
                new EmailValueObject("weslley.carneiro@outlook.com"));

            model.Id = entity.Id;

            var logger = new XunitLogger<ApiControllerBase<TestEntity, Guid>>();
            
            var mockApplicationService = new Mock<IBaseServiceApplication<TestEntity, Guid>>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<TestViewModel>(It.IsAny<TestEntity>())).Returns(model);
            mapperMock.Setup(mapper => mapper.Map<TestEntity>(It.IsAny<TestViewModel>())).Returns(entity);

            var controller = new TestController(logger, mockApplicationService.Object);
            
            //When
            await controller.GetAllAsync<TestViewModel>();
            await controller.GetByIdAsync<TestViewModel>(model.Id);
            await controller.InsertAsync<TestViewModel>(model);
            await controller.UpdateAsync<TestViewModel>(model);
            await controller.DeleteAsync(model.Id);

            //Then
            var msgContructor = "Inicializando Controller Base<TestEntity, Guid>";
            var msgGetById = $"Método: GetByIdAsync({{ id:{ entity.Id } }}) Retorno: type TestViewModel";
            var msgGetAllAsync = "Método: GetAllAsync() Retorno: IEnumerable<TestViewModel>";
            var msgInsertAsync = $"Método: InsertAsync({{ viewModel:{ model.ToJson() } }})";
            var msgUpdateAsync = $"Método: UpdateAsync({{ viewModel:{ model.ToJson() } }})";
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
