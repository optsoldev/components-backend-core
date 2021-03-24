using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optsol.Playground.Application.Mappers.Cliente;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Domain.Repositories.Cliente;
using Optsol.Playground.Infra.Data.Context;
using Optsol.Playground.Infra.Data.Repositories.Cliente;

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
            var connectionString = this.Configuration.GetSection("ConnectionStrings:DefaultConnection");

            services.AddContext<PlaygroundContext>(options =>
            {
                options
                    .ConfigureConnectionString(connectionString.Value)
                    .ConfigureMigrationsAssemblyName("Optsol.Playground.Infra")
                    .EnabledLogging();

            });
            services.AddRepository<IClienteReadRepository, ClienteReadRepository>("Optsol.Playground.Domain", "Optsol.Playground.Infra");
            services.AddApplications<IClienteServiceApplication, ClienteServiceApplication>("Optsol.Playground.Application");
            services.AddDomainNotifications();
            services.AddServices();
            
            services.AddCors(Configuration);
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

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseSecurity(Configuration);
                        
            app.UseCors(Configuration);

            app.UseSwagger(Configuration, env.IsDevelopment());

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
