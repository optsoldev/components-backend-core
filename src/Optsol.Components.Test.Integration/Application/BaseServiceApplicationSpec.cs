using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Data;
using Xunit;

namespace Optsol.Components.Test.Integration.Application
{
    public class BaseServiceApplicationSpec
    {

        [Fact]
        public async Task DeveTestarAConfiguracaoDoServico()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            //When
            services.AddLogging();
            services.AddAutoMapper(typeof(TestViewModel));
            services.AddRepository<TestContext>(new ContextOptionsBuilder());
            services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");
            services.RegisterApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");
            
            var provider = services.BuildServiceProvider();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
            IServiceApplication serviceApplication = provider.GetRequiredService<IServiceApplication>();
                        
            //Then
            await writeRepository.InsertAsync(entity);
            var entityGet = await serviceApplication.GetAllAsync<TestViewModel>();
            entityGet.Should().HaveCount(1);
            entityGet.First().Nome.Should().Be(entity.Nome.ToString());
            entityGet.First().Contato.Should().Be(entity.Email.ToString());
        }
    }
}
