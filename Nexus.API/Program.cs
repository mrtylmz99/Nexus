using Microsoft.EntityFrameworkCore;
using Nexus.Infrastructure;
using Nexus.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers(); // Add Controllers

// Clean Architecture: Infrastructure Services Registration
builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
