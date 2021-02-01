using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Optsol.Playground.Application.Mappers.Cliente;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Domain.Repositories.Cliente;
using Optsol.Playground.Infra.Data.Context;
using Optsol.Playground.Infra.Data.Repositories.Cliente;
using System.Collections.Generic;
using System.Reflection;

namespace Optsol.Playground.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var stringConnection = this.Configuration.GetSection("ConnectionStrings:DefaultConnection");

            services.AddControllers().ConfigureNewtonsoftJson();
            services.AddContext<PlaygroundContext>(new ContextOptionsBuilder(stringConnection.Value, "Optsol.Playground.Infra", Environment.IsDevelopment()));
            services.AddRepository<IClienteReadRepository, ClienteReadRepository>("Optsol.Playground.Domain", "Optsol.Playground.Infra");
            services.AddApplicationServices<IClienteServiceApplication, ClienteServiceApplication>("Optsol.Playground.Application");
            services.AddAServices();
            services.AddAutoMapper(typeof(ClienteViewModelToEntityMapper));
            services.AddSwagger(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(Configuration);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Playground API Started.");
                });
                endpoints.MapControllers();
            });

        }
    }
}
