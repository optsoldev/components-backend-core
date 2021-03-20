using Microsoft.Extensions.Hosting;
using Optsol.Components.Service.Programs;

namespace Optsol.Playground.Api
{
    public class Program : BaseProgram
    {
        public static void Main(string[] args) => Start<Startup>(CreateHostBuilder(args));

        public static IHostBuilder CreateHostBuilder(string[] args) => CreateHostBuilder<Startup>(args);
    }
}
