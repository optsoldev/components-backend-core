using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Data;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Test.Shared.Logger;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Entity.Entities;
using Optsol.Components.Test.Utils.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Unit.Application
{
    public class BaseServiceApplicationSpec
    {
        [Trait("Serviço de Aplicação", "Log de Ocorrências")]
        [Fact(DisplayName = "Deve registrar os logs no serviço ao obter todos os registros")]
        public async Task Deve_Registrar_Logs_No_Servico()
        {
            //Given
            var entity = new TestEntity();
            entity.Validate();

            var entity2 = new TestEntity();
            entity2.Validate();

            var model = new TestViewModel();
            model.Nome = "Weslley Carneiro";
            model.Contato = "weslley.carneiro@optsol.com.br";

            var insertModel = new InsertTestViewModel();
            insertModel.Nome = "Weslley Carneiro";
            insertModel.Contato = "weslley.carneiro@optsol.com.br";

            var updateModel = new UpdateTestViewModel();
            updateModel.Id = Guid.NewGuid();
            updateModel.Nome = "Weslley Carneiro";
            updateModel.Contato = "weslley.carneiro@optsol.com.br";

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<TestViewModel>(It.IsAny<TestEntity>())).Returns(model);
            mapperMock.Setup(mapper => mapper.Map<TestEntity>(It.IsAny<TestViewModel>())).Returns(entity);
            mapperMock.Setup(mapper => mapper.Map<TestEntity>(It.IsAny<InsertTestViewModel>())).Returns(entity);
            mapperMock.Setup(mapper => mapper.Map<TestEntity>(It.IsAny<UpdateTestViewModel>())).Returns(entity);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var readRepository = new Mock<IReadRepository<TestEntity, Guid>>();
            readRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(entity);
            readRepository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(new List<TestEntity> { entity, entity2 });

            var writeRepository = new Mock<IWriteRepository<TestEntity, Guid>>();

            var notificationContextMock = new Mock<NotificationContext>();

            var logger = new XunitLogger<BaseServiceApplication<TestEntity>>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            var service = new BaseServiceApplication<TestEntity>(
                mapperMock.Object,
                loggerFactoryMock.Object,
                unitOfWork.Object,
                readRepository.Object,
                writeRepository.Object,
                notificationContextMock.Object);

            //When
            await service.GetAllAsync<TestViewModel>();
            await service.GetByIdAsync<TestViewModel>(entity.Id);
            await service.InsertAsync<InsertTestViewModel, InsertTestViewModel>(insertModel);
            await service.UpdateAsync<UpdateTestViewModel, UpdateTestViewModel>(updateModel);
            await service.DeleteAsync(entity.Id);

            //Then
            var msgConstructor = $"Inicializando Application Service<{ entity.GetType().Name }, Guid>";
            var msgGetByIdAsync = $"Método: GetByIdAsync({{ id:{ entity.Id } }}) Retorno: type TestViewModel";
            var msgGetAllAsync = $"Método: GetAllAsync() Retorno: IEnumerable<{ model.GetType().Name }>";
            var msgInsertAsync = $"Método: InsertAsync({{ viewModel:{ insertModel.ToJson() } }})";
            var msgInsertAsyncMapper = $"Método: InsertAsync Mapper: { insertModel.GetType().Name } To: { entity.GetType().Name }";
            var msgUpdateAsync = $"Método: UpdateAsync({{ viewModel:{ updateModel.ToJson() } }})";
            var msgUpdateAsyncMapper = $"Método: UpdateAsync Mapper: { updateModel.GetType().Name } To: { entity.GetType().Name }";
            var msgDeleteAsync = $"Método: DeleteAsync({{ id:{ entity.Id } }})";

            logger.Logs.Should().HaveCount(11);
            logger.Logs.Any(a => a.Equals(msgConstructor)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetByIdAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetAllAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgInsertAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(msgInsertAsyncMapper)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgUpdateAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(msgUpdateAsyncMapper)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgDeleteAsync)).Should().BeTrue();
        }


        public class ObterTodosParams : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new []
                    {
                        new TestEntity (new NomeValueObject("Isaiah", "Sosa"), new EmailValueObject("justo.eu.arcu@Integervitaenibh.net")),
                        new TestEntity (new NomeValueObject("Hop", "Gross"), new EmailValueObject("Integer@magna.co.uk")),
                        new TestEntity (new NomeValueObject("Armand", "Villarreal"), new EmailValueObject("lorem.tristique@posuerevulputatelacus.ca")),
                    },
                    new []
                    {
                         new TestViewModel() { Nome = "Isaiah Sosa", Contato = "justo.eu.arcu@Integervitaenibh.net" },
                         new TestViewModel() { Nome = "Hop Gross", Contato = "Integer@magna.co.uk" },
                         new TestViewModel() { Nome = "Armand Villarreal", Contato = "lorem.tristique@posuerevulputatelacus.ca" }
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Serviço de Aplicação", "Log de Ocorrências")]
        [Theory(DisplayName = "Deve registrar os logs no serviço ao obter todos os registros")]
        [ClassData(typeof(ObterTodosParams))]
        public async Task Deve_Registrar_Logs_No_Servico_Ao_Obter_Todos_Registros(IEnumerable<TestEntity> entities, IEnumerable<TestViewModel> viewModels)
        {
            //Given
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<TestViewModel>>(It.IsAny<TestEntity>())).Returns(viewModels);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var readRepository = new Mock<IReadRepository<TestEntity, Guid>>();
            readRepository.Setup(setup => setup.GetAllAsync()).ReturnsAsync(entities);

            var writeRepository = new Mock<IWriteRepository<TestEntity, Guid>>();

            var notificationContextMock = new Mock<NotificationContext>();

            var logger = new XunitLogger<BaseServiceApplication<TestEntity>>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            var service = new BaseServiceApplication<TestEntity>(
                mapperMock.Object,
                loggerFactoryMock.Object,
                unitOfWork.Object,
                readRepository.Object,
                writeRepository.Object,
                notificationContextMock.Object);

            //When
            await service.GetAllAsync<TestViewModel>();

            //Then
            var msgConstructor = $"Inicializando Application Service<{ nameof(TestEntity) }, Guid>";
            var msgGetAllAsync = $"Método: GetAllAsync() Retorno: IEnumerable<{ nameof(TestViewModel) }>";
            

            logger.Logs.Should().HaveCount(3);
            logger.Logs.Any(a => a.Equals(msgConstructor)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetAllAsync)).Should().BeTrue();
        }
    }
}
