using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.Bus.Delegates;
using Optsol.Components.Infra.Bus.Services;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Entity.Entities;
using Xunit;

namespace Optsol.Components.Test.Integration.Infra.Bus;

public class EventBusRabbitMqSpec
{
    private static ServiceProvider GetProviderConfiguredServicesFromContext()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($@"Settings/appsettings.rabbitmq.json")
            .Build();

        var services = new ServiceCollection();

        services.AddLogging();
        services.AddRabbitMQ(configuration);

        var provider = services.BuildServiceProvider();

        return provider;
    }

    [Fact(Skip ="Teste Local")]
    public void DeveTestar()
    {
        var provider = GetProviderConfiguredServicesFromContext();


        var rabbitmq = provider.GetRequiredService<IEventBus>();

        rabbitmq.Publish(new TestEntity(new NomeValueObject("Weslley", "Carneiro"), new EmailValueObject("weslley.carneiro@optsol.com.br")));

        rabbitmq.Subscribe<TestEntity>(received: (ReceivedMessageEventArgs e) =>
        {
            e.Message.Should().NotBeNull();
        });
    }



}