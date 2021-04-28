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
            services.AddMongoRepository<ITestMongoReadRepository, TestMongoReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();

            return provider;
        }

        [Trait("Infraestrutura", "MongoDB Respositório de Leitura")]
        [Fact(DisplayName = "Deve obter todos registros pelo repositório")]
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
        [Fact(DisplayName = "Deve obter o registro pelo id", Skip = "mongo local docker test")]
        public async Task Deve_obter_Por_Id_Pelo_Repositorio()
        {
            //Given
            var numberItems = 3;
            var entity = default(TestEntity);

            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInMongoContext(numberItems, (entityList) =>
                {
                    entity = entityList.First();
                });

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
                    yield return new object[] { entity };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Infraestrutura", "MongoDB Repositório de Escrita")]
        [Theory(DisplayName = "Deve inserir o registro na base de dados", Skip = "mongo local docker test")]
        [ClassData(typeof(InserirNovosRegistrosParams))]
        public async Task Deve_Inserir_Registro_Pelo_Repositorio(TestEntity entity)
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext();
            var unitOfWork = provider.GetRequiredService<IMongoUnitOfWork>();
            var readRepository = provider.GetRequiredService<ITestMongoReadRepository>();
            var writeRepository = provider.GetRequiredService<ITestMongoWriteRepository>();

            //When
            await writeRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();

            //Then
            var entityResult = await readRepository.GetByIdAsync(entity.Id);
            entityResult.IsValid.Should().BeTrue();
            entityResult.Notifications.Should().HaveCount(0);
            entityResult.Should().NotBeNull();
            entityResult.Nome.ToString().Should().Be(entity.Nome.ToString());
            entityResult.Email.ToString().Should().Be(entity.Email.ToString());
            entityResult.Ativo.Should().BeFalse();
        }

        [Trait("Infraestrutura", "MongoDB Respositório de Escrita")]
        [Theory(DisplayName = "Deve atualizar o registro obtido da base de dados", Skip = "mongo local docker test")]
        [ClassData(typeof(InserirNovosRegistrosParams))]
        public async Task Deve_Atualizar_Registro_Pelo_Repositorio(TestEntity entity)
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext()
                .CreateTestEntitySeedInMongoContext(1);

            var unitOfWork = provider.GetRequiredService<IMongoUnitOfWork>();
            var testReadRepository = provider.GetRequiredService<ITestMongoReadRepository>();
            var testWriteRepository = provider.GetRequiredService<ITestMongoWriteRepository>();
            var mapper = provider.GetRequiredService<IMapper>();

            await testWriteRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();

            //When
            var updateResult = await testReadRepository.GetByIdAsync(entity.Id);
            updateResult = mapper.Map<TestEntity>(updateResult);

            var updateEntity = new TestEntity(updateResult.Id, new NomeValueObject(updateResult.Nome.Nome, "Atualizado"), new EmailValueObject(updateResult.Email.Email));

            await testWriteRepository.UpdateAsync(updateEntity);
            await unitOfWork.CommitAsync();

            //Then
            var entityResult = await testReadRepository.GetByIdAsync(entity.Id);
            entityResult.Should().NotBeNull();

            entityResult.IsValid.Should().BeTrue();
            entityResult.Notifications.Should().BeEmpty();

            entityResult.Nome.ToString().Should().Be(updateEntity.Nome.ToString());
            entityResult.Email.ToString().Should().Be(updateEntity.Email.ToString());
            entityResult.Ativo.Should().BeFalse();
        }
    }
}
