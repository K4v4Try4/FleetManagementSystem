using CarService.Business.Interfaces;
using CarService.Business.Kafka;
using CarService.Business.Services;
using CarService.Repository.Implementations;
using CarService.Repository.Interfaces;
using CarService.Repository.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Configurazione dei servizi dell'applicazione.
/// </summary>
/// <remarks>
/// In questa sezione vengono registrati:
/// - DbContext EF Core
/// - Repository layer
/// - Business layer
/// - Hosted services (Kafka consumer)
/// - Controller e strumenti API (Swagger)
/// </remarks>

// 1. Configurazione Database (SQL Server)
builder.Services.AddDbContext<CarDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrazione Dependency Injection
// Repository (Scoped: una istanza per richiesta HTTP)
builder.Services.AddScoped<ICarRepository, CarRepository>();

// Business Logic (Scoped: una istanza per richiesta HTTP)
builder.Services.AddScoped<ICarBusinessService, CarBusinessService>();

// 3. Registrazione del Consumer Kafka (Background Service)
/// <remarks>
/// Questo servizio gira in background e consuma eventi dal topic Kafka.
/// Non è legato al ciclo di vita delle richieste HTTP.
/// </remarks>
builder.Services.AddHostedService<TripCompletedConsumer>();

// 4. Configurazione Controller e Swagger
builder.Services.AddControllers().AddJsonOptions(options => {
        // Questo dice a .NET di convertire automaticamente le stringhe (es. "AVAILABLE") in Enum e viceversa
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/// <summary>
/// Configurazione della pipeline HTTP.
/// </summary>
/// <remarks>
/// Definisce middleware per:
/// - Swagger (solo Development)
/// - HTTPS redirection
/// - autorizzazione
/// - routing controller
/// </remarks>

// 5. Configurazione Middleware (Pipeline HTTP)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CarDbContext>(); // Usa il nome del tuo DbContext
    context.Database.Migrate(); // Questo "spara" le migrazioni sul server
}

app.Run();