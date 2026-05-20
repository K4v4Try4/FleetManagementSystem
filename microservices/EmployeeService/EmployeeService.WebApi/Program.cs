using EmployeeService.Business.Interfaces;
using EmployeeService.Business.Kafka;
using EmployeeService.Business.Services;
using EmployeeService.Repository.Implementations;
using EmployeeService.Repository.Interfaces;
using EmployeeService.Repository.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<EmployeeDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeBusinessService, EmployeeBusinessService>();
builder.Services.AddSingleton<IEmployeeEventProducer, EmployeeEventProducer>();

// Controllers
builder.Services.AddControllers();

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

// Automatic DB migrations on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
    context.Database.Migrate();
}

app.Run();