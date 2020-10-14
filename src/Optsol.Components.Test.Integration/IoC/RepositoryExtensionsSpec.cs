using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.IoC;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Utils.Data;

namespace Optsol.Components.Test.Integration.IoC
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
            services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>(); 
            ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();

            //Then
            await writeRepository.InsertAsync(entity);
            var entityGet = await readRepository.GetById(entity.Id);
            entityGet.Id.Should().Be(entity.Id);
            entityGet.ToString().Should().Be(entity.ToString());

        }
    }
    }
}
