using Microsoft.EntityFrameworkCore;
using Nexus.Infrastructure;
using Nexus.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.
builder.Services.AddOpenApi();

// Clean Architecture: Infrastructure Services Registration
builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
