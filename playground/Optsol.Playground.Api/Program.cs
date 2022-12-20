 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optsol.Playground.Application.Mappers.CartaoCredito;
using Optsol.Playground.Application.Services.Cliente;
using Optsol.Playground.Domain.Repositories.Cliente;
using Optsol.Playground.Infra.Data.Context;
using Optsol.Playground.Infra.Data.Repositories.Cliente;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

var connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection");

builder.Services.AddHealthChecks(configuration);

builder.Services.AddContext<PlaygroundContext>(options =>
{
    options
        .ConfigureConnectionString(connectionString.Value)
        .ConfigureMigrationsAssemblyName("Optsol.Playground.Infra")
        .EnabledLogging();

    options
        .ConfigureRepositories<IClientePessoaFisicaReadRepository, ClienteReadRepository>("Optsol.Playground.Domain", "Optsol.Playground.Infra");

});
builder.Services.AddApplications(options =>
{
    options
        .ConfigureAutoMapper<CartaoCreditoEntityToViewModelMapper>()
        .ConfigureServices<IClienteServiceApplication, ClienteServiceApplication>("Optsol.Playground.Application");
});
builder.Services.AddDomainNotifications();
builder.Services.AddServices();


builder.Services.AddCors(configuration);
builder.Services.AddSecurity(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseException(builder.Environment.IsDevelopment());

app.UseHttpsRedirection();

app.UseRouting();

app.UseSecurity(configuration);

app.UseCors(configuration);
            
app.UseHealthChecks(configuration);

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Playground API Started.");
    });

    endpoints.MapControllers();
});

app.Run();