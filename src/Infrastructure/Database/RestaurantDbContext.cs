using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Core.Entity;

namespace RestaurantAPI.Infrastructure.Database;

public class RestaurantDbContext : DbContext
{
	public DbSet<RestaurantEntity> Restaurants { get; set; }
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<DishEntity> Dishes { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserEntity> Users { get; set; }


    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
		: base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		OnRestaurantEntityCreating(modelBuilder);
		OnDishEntityCreating(modelBuilder);
		OnAddressEntityCreating(modelBuilder);
		OnUserEntityCreating(modelBuilder);
		OnRoleEntityCreating(modelBuilder);
	}

	private static void OnRestaurantEntityCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<RestaurantEntity>()
			.Property(r => r.Name)
			.IsRequired()
			.HasMaxLength(256);
	}

	private static void OnDishEntityCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DishEntity>()
			.Property(d => d.Name)
			.IsRequired()
			.HasMaxLength(256);
	}

	private static void OnAddressEntityCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<AddressEntity>()
			.Property(a => a.City)
			.HasMaxLength(256);

		modelBuilder.Entity<AddressEntity>()
			.Property(a => a.Street)
			.HasMaxLength(256);
	}

	private static void OnUserEntityCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserEntity>()
			.Property(x => x.Email)
			.IsRequired();

		modelBuilder.Entity<UserEntity>()
			.Property(x => x.Password)
			.IsRequired();
	}

	private static void OnRoleEntityCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<RoleEntity>()
			.Property(x => x.Name)
			.IsRequired();
	}
}
