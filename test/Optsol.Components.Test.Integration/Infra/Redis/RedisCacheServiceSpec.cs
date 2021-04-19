using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.Redis.Services;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Entity.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Integration.Infra.Redis
{
    public class RedisCacheServiceSpec
    {
        private static ServiceProvider GetProviderConfiguredServicesFromContext()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($@"Settings/appsettings.redis.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddRedisCache(configuration);

            var provider = services.BuildServiceProvider();

            return provider;
        }

        [Trait("Infraestrutura", "Redis")]
        [Fact(DisplayName = "Deve salvar o registro no cache", Skip = "redis local docker test")]
        public async Task DeveTestar()
        {
            var provider = GetProviderConfiguredServicesFromContext();

            var redisCacheService = provider.GetRequiredService<IRedisCacheService>();

            var testEntity = new TestEntity(new NomeValueObject("Weslley", "Carneiro"), new EmailValueObject("weslley.carneiro@optsol.com.br"));

            await redisCacheService.SaveAsync(new KeyValuePair<string, TestEntity>(testEntity.Id.ToString(), testEntity));

            var entityCache = await redisCacheService.ReadAsync<TestEntity>(testEntity.Id.ToString());

            entityCache.Should().NotBeNull();
        }
    }
}
