using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.Storage.Blob;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Shared.Settings;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Integration.Infra.Storage.Table
{
    public class BlobStorageSpec
    {

        [Fact(Skip = "azurite local docker test")]
        public void Deve_Registrar_Serico_Storage_Na_Injecao_De_Dependencia()
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
            Action blobStorage = () => provider.GetRequiredService<IBlobStorage>();

            //Then
            blobStorage.Should().NotThrow();
        }

        [Fact(Skip = "azurite local docker test")]
        public async Task Deve_Criar_Container_Blob_No_Azure_Storage()
        {
            //Given 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($@"Settings/appsettings.storage.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddStorage(configuration);

            var provider = services.BuildServiceProvider();
            var blobSettings = provider.GetRequiredService<StorageSettings>();
            var blobStorage = (BlobStorage)provider.GetRequiredService<IBlobStorage>();

            //When 
            var containerExiteNoAzureStorage = await blobStorage.ContainerExistsAsync();

            //Then
            blobSettings.Should().NotBeNull();
            blobStorage.Should().NotBeNull();

            containerExiteNoAzureStorage.Should().NotBeNull();
            containerExiteNoAzureStorage.GetRawResponse().Status.Should().Be(200);
            containerExiteNoAzureStorage.Value.Should().BeTrue();
        }

        [Fact(Skip = "azurite local docker test")]
        public void Deve_Fazer_Upload_De_Arquivo_No_Blob_Pelo_Stream()
        {
            //Given 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Settings/appsettings.storage.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddStorage(configuration);

            var provider = services.BuildServiceProvider();
            var blobStorage = (BlobStorage)provider.GetRequiredService<IBlobStorage>();

            Stream stream = File.OpenRead(@"Anexos/anexo.jpg");


            //When 
            Action action = () => blobStorage.UploadAsync($"{Guid.NewGuid()}.jpg", stream);

            //Then
            blobStorage.Should().NotBeNull();

            action.Should().NotThrow();
        }

        [Fact(Skip = "azurite local docker test")]
        public void Deve_Fazer_Upload_De_Arquivo_No_Blob_Pelo_Path()
        {
            //Given 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Settings/appsettings.storage.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddStorage(configuration);

            var provider = services.BuildServiceProvider();
            var blobStorage = (BlobStorage)provider.GetRequiredService<IBlobStorage>();

            //When 
            Action action = () => blobStorage.UploadAsync($"{Guid.NewGuid()}.jpg", @"Anexos/anexo.jpg");

            //Then
            blobStorage.Should().NotBeNull();

            action.Should().NotThrow();
        }

        [Fact(Skip = "azurite local docker test")]
        public async Task Deve_Apagar_Arquivo_No_Blob_Pelo_Nome()
        {
            //Given 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Settings/appsettings.storage.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddStorage(configuration);

            var provider = services.BuildServiceProvider();
            var blobStorage = (BlobStorage)provider.GetRequiredService<IBlobStorage>();

            var name = $"{Guid.NewGuid()}.jpg";
            await blobStorage.UploadAsync(name, @"Anexos/anexo.jpg");

            //When 
            Action action = () => blobStorage.DeleteAsync(name);

            //Then
            blobStorage.Should().NotBeNull();

            action.Should().NotThrow();
        }

        [Fact(Skip = "azurite local docker test")]
        public async Task Deve_Fazer_Download_Do_Arquivo_No_Blob_Pelo_Nome()
        {
            //Given 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Settings/appsettings.storage.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddStorage(configuration);

            var provider = services.BuildServiceProvider();
            var blobStorage = (BlobStorage)provider.GetRequiredService<IBlobStorage>();

            var name = $"{Guid.NewGuid()}.jpg";
            await blobStorage.UploadAsync(name, @"Anexos/anexo.jpg");

            //When 
            var arquivoDoBlob = await blobStorage.DowloadAsync(name);

            //Then
            blobStorage.Should().NotBeNull();

            arquivoDoBlob.Should().NotBeNull();
            arquivoDoBlob.GetRawResponse().Status.Should().Be(200);
            arquivoDoBlob.Value.ContentLength.Should().BeGreaterThan(0);
        }
    }
}
