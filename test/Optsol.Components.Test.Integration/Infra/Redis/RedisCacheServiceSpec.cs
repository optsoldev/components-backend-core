using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.Redis.Services;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Integration.Infra.Redis;

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
#if DEBUG
        [Fact(DisplayName = "Deve salvar o registro no cache")]
#elif RELEASE
        [Fact(DisplayName = "Deve salvar o registro no cache", Skip = "redis local docker test")]
#endif
        public void Deve_Salvar_Registro_Cache()
        {
                //given
                var provider = GetProviderConfiguredServicesFromContext();

                var redisCacheService = provider.GetRequiredService<IRedisCacheService>();

                var testEntity = new TestEntity(new NomeValueObject("Weslley", "Carneiro"), new EmailValueObject("weslley.carneiro@optsol.com.br"));

                //when
                Action execute = () => redisCacheService.SaveAsync(new KeyValuePair<string, TestEntity>(testEntity.Id.ToString(), testEntity));

                //then
                execute.Should().NotThrow();
        }

        [Trait("Infraestrutura", "Redis")]
#if DEBUG
        [Fact(DisplayName = "Deve salvar o registro no cache com timeout")]
#elif RELEASE
        [Fact(DisplayName = "Deve salvar o registro no cache com timeout", Skip = "redis local docker test")]
#endif
        public void Deve_Salvar_Registro_Cache_Timeout()
        {
                //given
                var provider = GetProviderConfiguredServicesFromContext();

                var redisCacheService = provider.GetRequiredService<IRedisCacheService>();

                var testEntity = new TestEntity(new NomeValueObject("Weslley", "Carneiro"), new EmailValueObject("weslley.carneiro@optsol.com.br"));

                //when
                Action execute = () => redisCacheService.SaveAsync(new KeyValuePair<string, TestEntity>(testEntity.Id.ToString(), testEntity), 1);

                //then
                execute.Should().NotThrow();
        }

        [Trait("Infraestrutura", "Redis")]
#if DEBUG
        [Fact(DisplayName = "Deve buscar o registro no cache")]
#elif RELEASE
        [Fact(DisplayName = "Deve buscar o registro no cache", Skip = "redis local docker test")]
#endif
        public async Task Deve_Buscar_Registro_Cache()
        {
                //given
                var provider = GetProviderConfiguredServicesFromContext();

                var redisCacheService = provider.GetRequiredService<IRedisCacheService>();

                var testEntity = new TestEntity(new NomeValueObject("Weslley", "Carneiro"), new EmailValueObject("weslley.carneiro@optsol.com.br"));

                await redisCacheService.SaveAsync(new KeyValuePair<string, TestEntity>(testEntity.Id.ToString(), testEntity));

                //when
                var entityCache = await redisCacheService.ReadAsync<TestEntity>(testEntity.Id.ToString());

                //then
                entityCache.Should().NotBeNull();
        }

        [Trait("Infraestrutura", "Redis")]
#if DEBUG
        [Fact(DisplayName = "Deve remover o registro no cache")]
#elif RELEASE
        [Fact(DisplayName = "Deve remover o registro no cache", Skip = "redis local docker test")]
#endif
        public async Task Deve_Remover_Registro_Cache()
        {
                //given
                var provider = GetProviderConfiguredServicesFromContext();

                var redisCacheService = provider.GetRequiredService<IRedisCacheService>();

                var testEntity = new TestEntity(new NomeValueObject("Weslley", "Carneiro"), new EmailValueObject("weslley.carneiro@optsol.com.br"));

                await redisCacheService.SaveAsync(new KeyValuePair<string, TestEntity>(testEntity.Id.ToString(), testEntity));
            
                //when
                await redisCacheService.DeleteAsync(testEntity.Id.ToString());

                //then
                var entityCache = await redisCacheService.ReadAsync<TestEntity>(testEntity.Id.ToString());
                entityCache.Should().BeNull();
        }
}