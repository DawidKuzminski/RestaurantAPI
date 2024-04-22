using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Infrastructure;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Middleware;
using RestaurantAPI.Infrastructure.Services;
using RestaurantAPI.Infrastructure.Services.Abstraction;
using RestaurantAPI.Infrastructure.Validation;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

var authSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authSettings);

builder.Services.AddAuthentication(opt =>
{
	opt.DefaultAuthenticateScheme = "Bearer";
	opt.DefaultScheme = "Bearer";
	opt.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
	cfg.RequireHttpsMetadata = false;
	cfg.SaveToken = true;
	cfg.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = authSettings.Issuer,
		ValidAudience = authSettings.Issuer,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Jwk))
	};
});

builder.Services.AddScoped<IAuthorizationHandler, ValidateMinAgeRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ValidateResourceOperationRequirementHandler>();
builder.Services.AddAuthorization(opt =>
{
	opt.AddPolicy("AtLeast16", builder => builder.AddRequirements(new ValidateMinAgeRequirement(16)));
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton(authSettings);
var databaseConnStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RestaurantDbContext>(opt => opt.UseSqlServer(databaseConnStr, x => x.MigrationsAssembly("RestaurantAPI.Infrastructure")));
builder.Services.AddScoped<InitDataSeeder>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
	InitDataSeeder seeder = scope.ServiceProvider.GetRequiredService<InitDataSeeder>();
	seeder.Seed();
}


app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API"));
app.UseAuthorization();

app.MapControllers();

app.Run();
