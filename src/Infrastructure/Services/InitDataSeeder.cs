using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Core.Model;
using RestaurantAPI.Infrastructure.Database;

namespace RestaurantAPI.Infrastructure.Services;

public class InitDataSeeder
{
	private readonly RestaurantDbContext _dbContext;

	public InitDataSeeder(RestaurantDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void Seed()
	{
		if (_dbContext.Database.CanConnect())
		{
			if(!_dbContext.Roles.Any()) 
			{
				_dbContext.Roles.AddRange(InitRoles());
				_dbContext.SaveChanges();
			}

			if (!_dbContext.Restaurants.Any())
			{
				_dbContext.Restaurants.AddRange(GetInitRestaurants());
				_dbContext.SaveChanges();
			}
		}
	}

	private IReadOnlyList<RestaurantEntity> GetInitRestaurants()
	{
		return new List<RestaurantEntity>
		{
			new RestaurantEntity
			{
				Name = "Zorro - Drink and Drink",
				Category = "Alcohol",
				Description = "Only alcohol - food not allowed.",
				ContactEmail = "contact@zorodad.com",
				ContactPhone = "333 333 333",
				HasDelivery = false,
				Dishes = new List<DishEntity>
				{
					new DishEntity
					{
						Name = "Sake",
						Price = 100,
						Description = "Alcohol"
					},
					new DishEntity
					{
						Name = "Vodka",
						Price = 100,
						Description = "Alcohol"
					}
				},
				Address = new AddressEntity
				{
					City = "Zielona Góra",
					Street = "Szklanki i szkoło 3",
					PostalCode = "33-333"
				}
			}
		};
	}

	private IReadOnlyList<RoleEntity> InitRoles()
	{
		return
		[
			new() { Name = Role.User.ToString(), Role = Role.User},
			new() { Name = Role.Manager.ToString(), Role = Role.Manager},
			new() { Name = Role.Admin.ToString(), Role = Role.Admin}
		];
	}
}