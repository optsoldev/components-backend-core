using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.Storage.Queue;
using Optsol.Components.Infra.Storage.Queue.Messages;
using Optsol.Components.Shared.Settings;
using Optsol.Components.Test.Utils.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Integration.Infra.Storage
{
    public class QueueStorageSpec
    {
        [Trait("Table Storage", "Queue")]
        [Fact(DisplayName = "Deve registrar o serviço IQueueStorage na injeção de dependência", Skip = "azurite local docker test")]
        public void Deve_Registrar_Servico_Storage_Queue_Na_Injecao_De_Dependencia()
        {
            //Given 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($@"Settings/appsettings.storage.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddStorage(configuration);

            var provider = services.BuildServiceProvider();

            //When 
            Action blobStorage = () => provider.GetRequiredService<IQueueStorage>();

            //Then
            blobStorage.Should().NotThrow();
        }

        [Trait("Table Storage", "Queue")]
        [Fact(DisplayName = "Deve enviar uma mensagem para fila", Skip = "azurite local docker test")]
        public async Task Deve_Enviar_Mensagem_Queue_No_Azure_Storage()
        {
            //Given 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($@"Settings/appsettings.storage.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddStorage(configuration);

            var viewModel = new TestViewModel();
            viewModel.Nome = "Weslley Carneiro";
            viewModel.Contato = "weslley.carneiro@optsol.com.br";

            var provider = services.BuildServiceProvider();
            var blobSettings = provider.GetRequiredService<StorageSettings>();
            var queueStorage = provider.GetRequiredService<IQueueStorage>();

            var mensagemGerada = string.Format("Olá {0}. Sua consulta foi agendada para o dia {1:dd/MM/yyyy 'às' HH:mm:ss}, quando chegar o dia acesse o portal através deste link: {2}", "Weslley Carneiro", DateTime.Now, "https://wwww.optsol.com.br");
            viewModel.Nome = mensagemGerada;

            var messageModel = new SendMessageModel<TestViewModel>
            {
                QueueName = "teste-queue",
                Data = viewModel
            };

            //When 
            var containerExiteNoAzureStorage = await queueStorage.SendMessageBase64Async(messageModel);

            //Then
            blobSettings.Should().NotBeNull();
            queueStorage.Should().NotBeNull();

            containerExiteNoAzureStorage.Should().NotBeNull();
            containerExiteNoAzureStorage.GetRawResponse().Status.Should().Be(201);
        }
    }
}
