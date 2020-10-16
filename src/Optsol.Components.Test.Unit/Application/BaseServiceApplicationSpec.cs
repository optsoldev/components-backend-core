using System.Runtime.InteropServices.ComTypes;
using System;
using AutoMapper;
using FluentAssertions;
using Moq;
using Optsol.Components.Application.Service;
using Optsol.Components.Domain;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Shared.Logger;
using Optsol.Components.Test.Utils.Application;
using Xunit;
using Optsol.Components.Shared.Extensions;
using System.Linq;

namespace Optsol.Components.Test.Unit.Application
{
    public class BaseServiceApplicationSpec
    {
        [Fact]
        public void DeveRegistrarLogsNoServico()
        {
            //Given
            var entity = new AggregateRoot();
            var model = new TestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";
            
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<TestViewModel>(It.IsAny<AggregateRoot>())).Returns(model);
            mapperMock.Setup(mapper => mapper.Map<AggregateRoot>(It.IsAny<TestViewModel>())).Returns(entity);
            
            Mock<IUnitOfWork> unitOfWork = new Mock<IUnitOfWork>();
            Mock<IReadRepository<AggregateRoot, Guid>> readRepository = new Mock<IReadRepository<AggregateRoot, Guid>>();
            Mock<IWriteRepository<AggregateRoot, Guid>> writeRepository = new Mock<IWriteRepository<AggregateRoot, Guid>>();

            var logger = new XunitLogger<BaseServiceApplication<AggregateRoot, Guid>>();
            var service = new BaseServiceApplication<AggregateRoot, Guid>(
                mapperMock.Object,
                logger,
                unitOfWork.Object,
                readRepository.Object,
                writeRepository.Object);

            //When
            service.GetByIdAsync<TestViewModel>(entity.Id).ConfigureAwait(false);
            service.GetAllAsync<TestViewModel>().ConfigureAwait(false);
            service.InsertAsync(model).ConfigureAwait(false);
            service.UpdateAsync(model).ConfigureAwait(false);
            service.DeleteAsync(entity.Id).ConfigureAwait(false);

            //Then
            var msgConstructor = "Inicializando Application Service<AggregateRoot, Guid>";
            var msgGetByIdAsync = $"Método: GetByIdAsync({{ id:{ entity.Id } }}) Retorno: type TestViewModel";
            var msgGetAllAsync = "Método: GetAllAsync() Retorno: IEnumerable<TestViewModel>";
            var msgInsertAsync = $"Método: InsertAsync({{ viewModel:{ model.ToJson() } }})";
            var msgInsertAsyncMapper = $"Método: InsertAsync Mapper: TestViewModel To: AggregateRoot Result: { entity.ToJson() }";
            var msgUpdateAsync = $"Método: UpdateAsync({{ viewModel:{ model.ToJson() } }})";
            var msgUpdateAsyncMapper = $"Método: UpdateAsync Mapper: TestViewModel To: AggregateRoot Result: { entity.ToJson() }";
            var msgDeleteAsync = $"Método: DeleteAsync({{ id:{ entity.Id } }})";

            logger.Logs.Should().HaveCount(8);
            logger.Logs.Any(a => a.Equals(msgConstructor)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetByIdAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetAllAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgInsertAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgInsertAsyncMapper)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgUpdateAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgUpdateAsyncMapper)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgDeleteAsync)).Should().BeTrue();
        }
    }
}
