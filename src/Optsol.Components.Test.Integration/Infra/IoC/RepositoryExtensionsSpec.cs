using Xunit;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Utils.Data;
using Optsol.Components.Shared.Extensions;
using System.Linq;

namespace Optsol.Components.Test.Integration.IoC
{
    public class RepositoryExtensionsSpec
    {
        public class RepositoryExtensionSpec
        {
            [Fact]
            public async Task Deve_Testar_Configuracao_Do_Repositorio()
            {
                //Given
                var services = new ServiceCollection();
                var entity = new TestEntity(
                    new NomeValueObject("Weslley", "Carneiro")
                    , new EmailValueObject("weslley.carneiro@optsol.com.br"));

                //When
                services.AddLogging();
                services.AddContext<TestContext>(new ContextOptionsBuilder());
                services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

                var provider = services.BuildServiceProvider();
                IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>(); 
                ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
                ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
                await writeRepository.InsertAsync(entity);
                await unitOfWork.CommitAsync();

                //Then                
                var entityGet = await readRepository.GetByIdAsync(entity.Id);
                entityGet.Id.Should().Be(entity.Id);
                entityGet.ToString().Should().Be(entity.ToString());
            }
        }
    }
}
