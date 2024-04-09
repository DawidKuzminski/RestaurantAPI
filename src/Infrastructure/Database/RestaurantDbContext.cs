using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Core.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RestaurantAPI.Infrastructure.Database;

public class RestaurantDbContext : DbContext
{
	public DbSet<RestaurantEntity> Restaurants { get; set; }
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<DishEntity> Dishes { get; set; }

    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
		: base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		OnRestaurantModelCreating(modelBuilder);
		OnDishModelCreating(modelBuilder);
		OnAddressModelCreating(modelBuilder);
	}

	private static void OnRestaurantModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<RestaurantEntity>()
			.Property(r => r.Name)
			.IsRequired()
			.HasMaxLength(256);
	}

	private static void OnDishModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DishEntity>()
			.Property(d => d.Name)
			.IsRequired()
			.HasMaxLength(256);
	}

	private static void OnAddressModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<AddressEntity>()
			.Property(a => a.City)
			.HasMaxLength(256);

		modelBuilder.Entity<AddressEntity>()
			.Property(a => a.Street)
			.HasMaxLength(256);
	}
}
