using EmployeeService.Business.Interfaces;
using EmployeeService.Business.Kafka;
using EmployeeService.Business.Services;
using EmployeeService.Repository.Implementations;
using EmployeeService.Repository.Interfaces;
using EmployeeService.Repository.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Configurazione dei servizi dell'applicazione.
/// </summary>

// 1. Database
builder.Services.AddDbContext<EmployeeDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Dependency Injection (layer repository + business + eventi)
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeBusinessService, EmployeeBusinessService>();
builder.Services.AddSingleton<IEmployeeEventProducer, EmployeeEventProducer>();

// 3. MVC / Controllers
builder.Services.AddControllers();

// 4. Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/// <summary>
/// Configurazione della pipeline HTTP dell'applicazione.
/// </summary>

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Esecuzione migrazioni automatiche al bootstrap dell'applicazione
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EmployeeDbContext>();
    context.Database.Migrate();
}

app.Run();