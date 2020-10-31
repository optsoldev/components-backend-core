using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Optsol.Playground.Domain.Repositories;
using Optsol.Playground.Infra.Data.Context;

namespace Optsol.Playground.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var stringConnection = this.Configuration.GetSection("ConnectionStrings:DefaultConnection");

            services.AddControllers();
            services.AddContext<PlaygroundContext>(new ContextOptionsBuilder(stringConnection.Value, "Optsol.Playground.Infra"));
            // services.AddAutoMapper(typeof(TestViewModel));
            // services.AddRepository<IClienteReadRepository>("Optsol.Playground.Infra");
            // services.AddApplicationServices<IServiceApplication>("Optsol.Components.Test.Utils");
            // services.AddAServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
