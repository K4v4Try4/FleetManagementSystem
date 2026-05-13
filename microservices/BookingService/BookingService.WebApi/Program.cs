using BookingService.Business.Interfaces;
using BookingService.Business.Kafka;
using BookingService.Business.Services;
using BookingService.Repository.Implementations;
using BookingService.Repository.Interfaces;
using BookingService.Repository.Persistence;
using CarService.ClientHttp.Implementations;
using CarService.ClientHttp.Interfaces;
using EmployeeService.ClientHttp.Implementations;
using EmployeeService.ClientHttp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Configurazione dell'applicazione BookingService.
/// Registra dipendenze, database, client HTTP e middleware.
/// </summary>

// 1. Configurazione Database (SQL Server)
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrazione Client HTTP (comunicazione sincrona con servizi esterni)
builder.Services.AddHttpClient<ICarClient, CarClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalServices:CarServiceUrl"]
        ?? "http://localhost:5001");
});

builder.Services.AddHttpClient<IEmployeeClient, EmployeeClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalServices:EmployeeServiceUrl"]
        ?? "http://localhost:5002");
});

// 3. Dependency Injection: Repository e Business Layer
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingBusinessService, BookingBusinessService>();

// 4. Kafka Producer (comunicazione asincrona event-driven)
builder.Services.AddSingleton<IBookingEventProducer, BookingEventProducer>();

// 5. Controller + configurazione JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Serializzazione enum come stringhe (es. ACTIVE invece di 0)
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 6. Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 7. Applicazione automatica migrazioni database al bootstrap
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
    context.Database.Migrate();
}

app.Run();