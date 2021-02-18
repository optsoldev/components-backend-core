using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optsol.Components.Infra.Security.Attributes;
using Optsol.Playground.Application.Mappers.Cliente;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Domain.Repositories.Cliente;
using Optsol.Playground.Infra.Data.Context;
using Optsol.Playground.Infra.Data.Repositories.Cliente;
using System;

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

        public void ConfigureServices(IServiceCollection services)
        {
            var stringConnection = this.Configuration.GetSection("ConnectionStrings:DefaultConnection");

            services
                .AddControllers()
                .ConfigureNewtonsoftJson();

            services.AddCors(Configuration);

            services.AddContext<PlaygroundContext>(new ContextOptionsBuilder(stringConnection.Value, "Optsol.Playground.Infra", Environment.IsDevelopment()));
            services.AddRepository<IClienteReadRepository, ClienteReadRepository>("Optsol.Playground.Domain", "Optsol.Playground.Infra");
            services.AddApplicationServices<IClienteServiceApplication, ClienteServiceApplication>("Optsol.Playground.Application");
            services.AddDomainNotifications();
            services.AddApiServices();

            services.AddSecurity(Configuration);
            services.AddSwagger(Configuration);

            services.AddAutoMapper(typeof(ClienteViewModelToEntityMapper));
        }
                
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(Configuration);

            app.UseSwagger(Configuration, env.IsDevelopment());

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Playground API Started.");
                });

                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
