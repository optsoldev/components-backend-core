using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Infra.MongoDB.Repositories;
using Optsol.Components.Shared.Settings;
using Optsol.Components.Test.Shared.Logger;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Unit.Infra.MongoDB
{
    public class MongoRepositorySpec
    {
        [Trait("Repository", "Log de Ocorrências")]
        [Fact(DisplayName = "Deve registrar logs no repositório do MongoDB", Skip = "mongo local docker test")]
        public async Task Deve_Registrar_Logs_No_Repositorio_MongoDB()
        {
            //Given
            var dataBaseName = "mongo-auto-create";
            var entity = new AggregateRoot();

            var mongoSettings = new MongoSettings
            {
                DatabaseName = dataBaseName,
                ConnectionString = "mongodb://127.0.0.1:27017"
            };

            var setMock = new Mock<IMongoCollection<AggregateRoot>>();
            var mongoContextMock = new Mock<MongoContext>(mongoSettings);
            
            var logger = new XunitLogger<MongoRepository<AggregateRoot, Guid>>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            var repositoryMock = new MongoRepository<AggregateRoot, Guid>(mongoContextMock.Object, loggerFactoryMock.Object);
            
            //When
            await repositoryMock.GetByIdAsync(entity.Id);
            await repositoryMock.GetAllAsync();
            await repositoryMock.InsertAsync(entity);
            await repositoryMock.UpdateAsync(entity);
            await repositoryMock.DeleteAsync(entity);
            await repositoryMock.DeleteAsync(entity.Id);
            await repositoryMock.SaveChangesAsync();

            //Then
            var msgContructor = "Inicializando MongoRepository<AggregateRoot, Guid>";
            var msgGetById = $"Método: GetByIdAsync( {{id:{ entity.Id }}} ) Retorno: type AggregateRoot";
            var msgGetAllAsync = "Método: GetAllAsync() Retorno: IAsyncEnumerable<AggregateRoot>";
            var msgInsertAsync = $"Método: InsertAsync( {{entity:{ entity.ToJson()}}} )";
            var msgUpdateAsync = $"Método: UpdateAsync( {{entity:{ entity.ToJson()}}} )";
            var msgDeleteAsync = $"Método: DeleteAsync( {{entity:{ entity.ToJson()}}} )";
            var msgDeleteNotFoundAsync = $"Método: DeleteAsync( {{id:{ entity.Id }}} ) Registro não encontrado";
            var msgSaveChanges = "Método: SaveChangesAsync()";

            logger.Logs.Should().HaveCount(9);
            logger.Logs.Any(a => a.Equals(msgGetById)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgContructor)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetAllAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgInsertAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgUpdateAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgDeleteAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgDeleteNotFoundAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgSaveChanges)).Should().BeTrue();

            mongoContextMock.Object.MongoClient.DropDatabase(dataBaseName);
            mongoContextMock.Object.Dispose();
        }
    }
}
