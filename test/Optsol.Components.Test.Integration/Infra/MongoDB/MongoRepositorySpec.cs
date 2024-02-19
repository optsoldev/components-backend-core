using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.MongoDB.UoW;
using Optsol.Components.Test.Utils.Data.Contexts;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Data.Repositories.Mongo;
using Optsol.Components.Test.Utils.Entity.Entities;
using Optsol.Components.Test.Utils.Mapper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.MongoDB.Repositories;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Shared.Settings;
using Optsol.Components.Test.Shared.Logger;
using Xunit;
using static Optsol.Components.Test.Utils.Seed.Seed;

namespace Optsol.Components.Test.Integration.Infra.MongoDB
{
    public class MongoRepositorySpec
    {
        private static ServiceProvider GetProviderConfiguredServicesFromContext()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($@"Settings/appsettings.mongo.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddMongoContext<MongoContext>(configuration);
            services.AddAutoMapper(typeof(TestViewModelToEntity));
            services.AddMongoRepository<ITestMongoReadRepository, TestMongoReadRepository>(
                "Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();

            return provider;
        }

        [Trait("Infraestrutura", "MongoDB Respositório de Leitura")]
#if DEBUG
        [Fact(DisplayName = "Deve obter todos registros pelo repositório")]
#elif RELEASE
        [Fact(DisplayName = "Deve obter todos registros pelo repositório", Skip = "mongo local docker test")]
#endif
        public async Task Deve_obter_Todos_Pelo_Repositorio()
        {
            //Given
            var numberItems = 3;
            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInMongoContext(numberItems);

            var testReadRepository = provider.GetRequiredService<ITestMongoReadRepository>();

            //When
            var entityResult = await testReadRepository.GetAllAsync();

            //Then
            entityResult.Should().HaveCount(numberItems);
        }

        [Trait("Infraestrutura", "MongoDB Respositório de Leitura")]
#if DEBUG
        [Fact(DisplayName = "Deve obter o registro pelo id")]
#elif RELEASE
        [Fact(DisplayName = "Deve obter o registro pelo id", Skip = "mongo local docker test")]
#endif
        public async Task Deve_obter_Por_Id_Pelo_Repositorio()
        {
            //Given
            var numberItems = 3;
            var entity = default(TestEntity);

            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInMongoContext(numberItems, (entityList) => { entity = entityList.First(); });

            var testReadRepository = provider.GetRequiredService<ITestMongoReadRepository>();

            //When
            var entityResult = await testReadRepository.GetByIdAsync(entity.Id);

            //Then
            entityResult.Should().NotBeNull();
            entityResult.Nome.ToString().Should().Be(entity.Nome.ToString());
            entityResult.Email.ToString().Should().Be(entity.Email.ToString());
        }

        public class InserirNovosRegistrosParams : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (var entity in TestEntityList().Take(3))
                {
                    yield return new object[] {entity};
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Infraestrutura", "MongoDB Repositório de Escrita")]
#if DEBUG
        [Theory(DisplayName = "Deve inserir o registro na base de dados")]
#elif RELEASE
        [Theory(DisplayName = "Deve inserir o registro na base de dados", Skip = "mongo local docker test")]
#endif
        [ClassData(typeof(InserirNovosRegistrosParams))]
        public async Task Deve_Inserir_Registro_Pelo_Repositorio(TestEntity entity)
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext();
            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            var readRepository = provider.GetRequiredService<ITestMongoReadRepository>();
            var writeRepository = provider.GetRequiredService<ITestMongoWriteRepository>();

            //When
            await writeRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();

            //Then
            var entityResult = await readRepository.GetByIdAsync(entity.Id);
            entityResult.Valid.Should().BeTrue();
            entityResult.Invalid.Should().BeFalse();
            entityResult.Notifications.Should().HaveCount(0);
            entityResult.Should().NotBeNull();
            entityResult.Nome.ToString().Should().Be(entity.Nome.ToString());
            entityResult.Email.ToString().Should().Be(entity.Email.ToString());
            entityResult.Ativo.Should().BeFalse();
        }

        [Trait("Infraestrutura", "MongoDB Respositório de Escrita")]
#if DEBUG
        [Theory(DisplayName = "Deve atualizar o registro obtido da base de dados")]
#elif RELEASE
        [Theory(DisplayName = "Deve atualizar o registro obtido da base de dados", Skip = "mongo local docker test")]
#endif
        [ClassData(typeof(InserirNovosRegistrosParams))]
        public async Task Deve_Atualizar_Registro_Pelo_Repositorio(TestEntity entity)
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInMongoContext(1);

            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            var testReadRepository = provider.GetRequiredService<ITestMongoReadRepository>();
            var testWriteRepository = provider.GetRequiredService<ITestMongoWriteRepository>();
            var mapper = provider.GetRequiredService<IMapper>();

            await testWriteRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();

            //When
            var updateResult = await testReadRepository.GetByIdAsync(entity.Id);
            updateResult = mapper.Map<TestEntity>(updateResult);

            var updateEntity = new TestEntity(updateResult.Id,
                new NomeValueObject(updateResult.Nome.Nome, "Atualizado"),
                new EmailValueObject(updateResult.Email.Email));

            await testWriteRepository.UpdateAsync(updateEntity);
            await unitOfWork.CommitAsync();

            //Then
            var entityResult = await testReadRepository.GetByIdAsync(entity.Id);
            entityResult.Should().NotBeNull();

            entityResult.Valid.Should().BeTrue();
            entityResult.Invalid.Should().BeFalse();
            entityResult.Notifications.Should().BeEmpty();

            entityResult.Nome.ToString().Should().Be(updateEntity.Nome.ToString());
            entityResult.Email.ToString().Should().Be(updateEntity.Email.ToString());
            entityResult.Ativo.Should().BeFalse();
        }
        
        [Trait("Repository", "Log de Ocorrências")]
#if DEBUG
        [Fact(DisplayName = "Deve registrar logs no repositorio do MongoDB")]
#elif RELEASE
        [Fact(DisplayName = "Deve registrar logs no repositorio do MongoDB", Skip = "mongo local docker test")]
#endif
        public async Task Deve_Registrar_Logs_No_Repositorio_MongoDB()
        {
            //Given
            var dataBaseName = "mongo-auto-create";
            var entity = new AggregateRoot();

            var mongoSettings = new MongoSettings
            {
                DatabaseName = dataBaseName,
                ConnectionString = "mongodb://127.0.0.1:30001"
            };

            var loggerContextFactoryMock = new Mock<ILoggerFactory>();
            var loggerContext = new XunitLogger<MongoContext>();
            loggerContextFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(loggerContext);

            var setMock = new Mock<IMongoCollection<AggregateRoot>>();
            var mongoContextMock = new Mock<MongoContext>(mongoSettings, loggerContextFactoryMock.Object);
            
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
    
     

