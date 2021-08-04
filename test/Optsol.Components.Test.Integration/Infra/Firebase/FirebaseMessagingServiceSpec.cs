using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Test.Utils.Entity.Entities;
using System.Threading.Tasks;
using Xunit;

namespace Optsol.Components.Test.Integration.Infra.Firebase
{
    public class FirebaseMessagingServiceSpec
    {
        private static ServiceProvider GetProviderConfiguredServicesFromContext()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($@"Settings/appsettings.firebase.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddFirebase(configuration);

            var provider = services.BuildServiceProvider();

            return provider;
        }

        [Trait("Infraestrutura", "Firebase")]
#if DEBUG 
        [Fact(DisplayName = "Deve testar")]
#elif RELEASE
        [Fact(DisplayName = "Deve obter todos registros pelo repositório", Skip = "mongo local docker test")]
#endif
        public async Task Deve_Testar()
        {
            //Given
            var provider = GetProviderConfiguredServicesFromContext();
            
            var testNotificationEntity = new TestNotificationEntity();


            var pushService = provider.GetRequiredService<IPushService>();

            //When
            await pushService.SendAsync(testNotificationEntity);

            //Then

        }
    }
}
