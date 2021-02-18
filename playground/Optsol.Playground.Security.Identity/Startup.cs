using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Optsol.Components.Infra.Security.Attributes;
using Optsol.Playground.Security.Identity.Services;
using Optsol.Security.Identity.Data;
using System.Collections.Generic;
using System.Reflection;

namespace Optsol.Playground.Security.Identity
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
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddCors(Configuration);

            services.AddMvc(options => { options.EnableEndpointRouting = false; });

            services.AddSecurity(Configuration, migrationAssembly, Environment.IsDevelopment(), options =>
            {
                options.AddSecurityDataService<SecurityDataService>();

                options.AddProfileService<ProfileService>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(Configuration);

            app.UseHttpsRedirection();

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "index.html" },
            });

            app.UseStaticFiles();

            app.UseSecurity();

            app.ConfigureSecurity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "catch-all",
                    template: "{*url}",
                    defaults: new { controller = "Default", action = "Index" }
                );
            });
        }
    }
}
