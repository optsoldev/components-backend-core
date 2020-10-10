using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Sdk.Infra.IoC;
using Optsol.Sdk.Infra.UoW;
using Optsol.Sdk.Test.Shared.Data;
using Optsol.Sdk.Test.Utils.Data;
using Xunit;

namespace Optsol.Sdk.Test.Integration.IoC
{
    public class RepositoryExtensionsSpec
    {
        public class RepositoryExtensionSpec
    {
        [Fact]
        public async Task DeveTestarConfiguracaoDoRepositorio()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            //When
            services.AddLogging();
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterRepositories<ITestReadRepository>("Optsol.Sdk.Test.Utils");

            var provider = services.BuildServiceProvider();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>(); 
            ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
            await writeRepository.InsertAsync(entity);

            //Then
            var entityGet = await readRepository.GetById(entity.Id);
            entityGet.Id.Should().Be(entity.Id);
            entityGet.ToString().Should().Be(entity.ToString());

        }
    }
    }
}
