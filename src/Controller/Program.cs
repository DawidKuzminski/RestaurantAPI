using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Middleware;
using RestaurantAPI.Infrastructure.Services;
using RestaurantAPI.Infrastructure.Services.Abstraction;

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();


// Add services to the container.
builder.Services.AddControllers();

var databaseConnStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RestaurantDbContext>(opt => opt.UseSqlServer(databaseConnStr));
builder.Services.AddScoped<RestaurantSeeder>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
	RestaurantSeeder seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
	seeder.Seed();
}


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API"));
app.UseAuthorization();

app.MapControllers();

app.Run();
