using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.ElasticSearch.Context;
using Optsol.Components.Infra.ElasticSearch.UoW;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Data.Repositories.Elastic;
using Optsol.Components.Test.Utils.Entity.Entities;
using Optsol.Components.Test.Utils.Mapper;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Integration.Infra.ElasticSearch
{
    public class ElasticRepositorySpec
    {
        private static ServiceProvider GetProviderConfiguredServicesFromContext()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($@"Settings/appsettings.elastic.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddElasticContext<ElasticContext>(configuration);
            services.AddAutoMapper(typeof(TestViewModelToEntity));
            services.AddElasticRepository<ITestElasticReadRepository, TestElasticReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();

            return provider;
        }

        [Fact(Skip ="Teste Local")]
        public async Task DeveTestar()
        {
            var provider = GetProviderConfiguredServicesFromContext();

            var entity = new TestEntity(new NomeValueObject("Weslley", "Bruno"), new EmailValueObject("weslley.carneiro@optsol.com.br"));

            var readRepository = provider.GetService<ITestElasticReadRepository>();
            var writeRepository = provider.GetService<ITestElasticWriteRepository>();
            var uow = provider.GetService<IElasticUnitOfWork>();

            //await writeRepository.InsertAsync(entity);
            //await uow.CommitAsync();

            //var entity2 = await readRepository.GetByIdAsync(entity.Id);
            //entity2 = new TestEntity(entity2.Id, new NomeValueObject("José", "Carmo"), entity2.Email);
            //await writeRepository.UpdateAsync(entity2);
            ////await uow.CommitAsync();


            //await writeRepository.DeleteAsync(entity.Id);
            //await uow.CommitAsync();

            var all = await readRepository.GetAllAsync();
        }
    }
}
