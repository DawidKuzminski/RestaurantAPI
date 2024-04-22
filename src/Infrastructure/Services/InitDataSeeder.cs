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
		}
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