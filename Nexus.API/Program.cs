using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Nexus.Infrastructure;
using Nexus.Infrastructure.Persistence;
using Nexus.Application;
using Serilog;
using Scalar.AspNetCore;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers(); // Add Controllers

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// JWT Authentication Configuration / JWT Kimlik Doğrulama Yapılandırması
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
    // Ensure this comes from Microsoft.AspNetCore.Authentication.JwtBearer
    // Bu ayarların JwtBearer kütüphanesinden geldiğinden emin olun
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Clean Architecture: Application Services Registration
builder.Services.AddApplicationServices();

// Clean Architecture: Include FluentValidation AutoValidation
builder.Services.AddFluentValidationAutoValidation();

// Clean Architecture: Algorithm Services Registration
// Temiz Mimari: Altyapı Servislerinin Kaydı
builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Nexus API")
               .WithTheme(ScalarTheme.DeepSpace)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            // Add HTTP Bearer Authentication (JWT) support to Scalar UI
            // Scalar UI için HTTP Bearer (JWT) Kimlik Doğrulama desteği
               //.WithPreferredScheme("Bearer") 
               //.WithHttpBearerAuthentication(bearer =>
               //{
               //    bearer.Token = "your-jwt-token-here";
               //});
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp"); // Enable CORS
app.UseAuthentication(); // Enable Auth
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("--> Seeding Database...");
// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<NexusDbContext>();
        DbInitializer.Initialize(context);
        Console.WriteLine("--> Database Seeded Successfully!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
        Console.WriteLine($"--> Error seeding DB: {ex.Message}");
    }
}

Console.WriteLine("--> Starting Web Application...");
app.Run();
