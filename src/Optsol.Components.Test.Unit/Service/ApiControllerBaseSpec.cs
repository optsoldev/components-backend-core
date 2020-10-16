using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
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
            var entity = new TestEntity();
            var model = new TestViewModel();
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
        }
    }
}
