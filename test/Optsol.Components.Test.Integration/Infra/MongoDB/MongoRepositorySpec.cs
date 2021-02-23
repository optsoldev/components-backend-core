using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.MongoDB.UoW;
using Optsol.Components.Test.Utils.Data;
using Optsol.Components.Test.Utils.Data.Context;
using Optsol.Components.Test.Utils.Data.Mongo;
using Optsol.Components.Test.Utils.Entity;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Integration.Infra.MongoDB
{
    public class MongoRepositorySpec
    {
        [Fact(Skip ="Integração com mongodb somente local")]
        public async Task Deve_Inserir_Registro_Pelo_Repositorio()
        {
            //Given
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($@"Settings/appsettings.mongo.json")
                .Build();

            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            services.AddLogging();
            services.AddMongoContext<TestMongoContext>(configuration);
            services.AddMongoRepository<ITestMongoReadRepository, TestMongoReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IMongoUnitOfWork unitOfWork = provider.GetRequiredService<IMongoUnitOfWork>();
            ITestMongoReadRepository readRepository = provider.GetRequiredService<ITestMongoReadRepository>();
            ITestMongoWriteRepository writeRepository = provider.GetRequiredService<ITestMongoWriteRepository>();

            //When
            await writeRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();

            //Then
            var entityResult = await readRepository.GetByIdAsync(entity.Id);
            entityResult.Invalid.Should().BeFalse();
            entityResult.Notifications.Should().HaveCount(0);
            entityResult.Should().NotBeNull();
            entityResult.Nome.ToString().Should().Be(entity.Nome.ToString());
            entityResult.Email.ToString().Should().Be(entity.Email.ToString());
            entityResult.Ativo.Should().BeFalse();
        }
    }
}
