using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var databaseConnStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RestaurantDbContext>(opt => opt.UseSqlServer(databaseConnStr));
builder.Services.AddScoped<RestaurantSeeder>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
	RestaurantSeeder seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
	seeder.Seed();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
