using CarService.Business.Interfaces;
using CarService.Business.Kafka;
using CarService.Business.Services;
using CarService.Repository.Implementations;
using CarService.Repository.Interfaces;
using CarService.Repository.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<CarDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarBusinessService, CarBusinessService>();

// Kafka consumer background service
builder.Services.AddHostedService<TripCompletedConsumer>();

// Controllers + JSON enum serialization
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Serialize enums as strings instead of integers
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Apply pending EF Core migrations at startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CarDbContext>();
    context.Database.Migrate();
}

app.Run();