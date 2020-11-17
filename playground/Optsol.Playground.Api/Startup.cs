using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Application.Mappers.Cliente;
using Optsol.Playground.Infra.Data.Context;
using Optsol.Playground.Infra.Data.Repositories.Cliente;

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
            services.AddRepository<IClienteReadRepository>("Optsol.Playground.Infra");
            services.AddApplicationServices<IClienteServiceApplication>("Optsol.Playground.Application");
            services.AddAServices();
            services.AddAutoMapper(typeof(ClienteViewModelToEntityMapper));
            services.AddMediatR(typeof(Startup));

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
