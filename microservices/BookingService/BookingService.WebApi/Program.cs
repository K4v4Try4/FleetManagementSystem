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

// Database
builder.Services.AddDbContext<BookingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// HTTP clients (sync integration with external services)
builder.Services.AddHttpClient<ICarClient, CarClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalServices:CarServiceUrl"] ?? "http://localhost:5001");
});

builder.Services.AddHttpClient<IEmployeeClient, EmployeeClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalServices:EmployeeServiceUrl"] ?? "http://localhost:5002");
});

// Dependency Injection
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingBusinessService, BookingBusinessService>();

// Eventing (Kafka)
builder.Services.AddSingleton<IBookingEventProducer, BookingEventProducer>();

// Controllers + JSON config
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// DB migrations at startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
    context.Database.Migrate();
}

app.Run();